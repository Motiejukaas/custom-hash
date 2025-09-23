using System.Diagnostics;
using System.Text;
using custom_hash.Utils;

namespace custom_hash.Tests;

public class Test
{

    private readonly Hash.Hash hash = new Hash.Hash();
    private readonly Comparer comparer = new Comparer();
    private readonly Converter converter = new Converter();
    
    //TODO incorrect for md5
    private const int HashLengthBit = 256;
    private const int HashLengthHex = 64;
    private const int HashLengthByte = 32;
    
    private static readonly string exeDir = AppContext.BaseDirectory;
    private static readonly string projectDir = Directory.GetParent(exeDir)
        .Parent
        .Parent
        .Parent
        .FullName;
    private static readonly string InputPairsDir = Path.Combine(projectDir, "Data", "RandomPairs");
    private static readonly string InputConstitutionDir = Path.Combine(projectDir, "Data", "konstitucija.txt");
    private static readonly string OutputDir = Path.Combine(projectDir, "Data", "Output");
    
    private readonly Func<byte[], byte[]> hashFunc;

    public Test(Func<byte[], byte[]> hashFunc)
    {
        this.hashFunc = hashFunc;
    }

    private byte[] ComputeHash(byte[] input) => hashFunc(input);
    
    public void RunTests()
    {
        Console.WriteLine("=== Hash Algorithm Test Suite ===");
        
        // 1. Fixed length
        double fixedLengthAccuracy = FixedLengthTest();
        Console.WriteLine($"[INFO] Fixed length accuracy: {fixedLengthAccuracy}% ");

        // 2. Determinism
        double determinismAccuracy = DeterminismTest();
        Console.WriteLine($"[INFO] Determinism accuracy: {determinismAccuracy}% ");

        //3. Speed
        SpeedTest();
        
        // 4. Collision
        double collisionAccuracy = CollisionTest();
        Console.WriteLine($"[INFO] Collision accuracy: {determinismAccuracy}% ");
        
        // 5. Avalanche effect
        var avalancheResults = AvalancheTest();
        Console.WriteLine("[INFO] Avalanche effect accuracy:");
        Console.WriteLine($"Bits: min: {avalancheResults.minBitPct}%, max: {avalancheResults.maxBitPct}%, avg: {avalancheResults.avgBitPct}% ");
        Console.WriteLine($"Hex:  min: {avalancheResults.minHexPct}%, max: {avalancheResults.maxHexPct}%, avg: {avalancheResults.avgHexPct}% ");
        
        Console.WriteLine("=== End of Tests ===");
    }
    
    private double FixedLengthTest()
    {
        int correct = 0, testCount = 0;
        
        //empty test
        byte[] hEmpty = ComputeHash(Array.Empty<byte>());
        if (hEmpty.Length == HashLengthByte)
        {
            correct++;
        }
        testCount++;
        
        foreach (var file in Directory.GetFiles(InputPairsDir, "RandomPairs*.txt"))
        {
            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                var pairs = line.Split(' ', 2);
                foreach (var s in pairs)
                {
                    byte[] h = ComputeHash(Encoding.UTF8.GetBytes(s));
                    if (h.Length == HashLengthByte) correct++;
                    testCount++;
                }
            }
        }
        
        return (correct / (double) testCount) * 100;
    }

    private double DeterminismTest()
    {
        int correct = 0, testCount = 0;

        foreach (var file in Directory.GetFiles(InputPairsDir, "RandomPairs*.txt"))
        {
            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                var parts = line.Split(' ', 2);
                foreach (var s in parts)
                {
                    var input = Encoding.UTF8.GetBytes(s);
                    var h1 = ComputeHash(input);
                    var h2 = ComputeHash(input);

                    if (h1.SequenceEqual(h2))
                    {
                        correct++;
                    }
                    testCount++;
                }
            }
        }

        return (correct / (double) testCount) * 100;
    }

    private void SpeedTest()
    {
        if (!File.Exists(InputConstitutionDir))
        {
            Console.WriteLine("[SKIP] Speed test (constitution.txt not found)");
            return;
        }

        string[] lines = File.ReadAllLines(InputConstitutionDir, Encoding.UTF8);
        List<(int lines, long avgTimeMs)> speedResults = [];
        
        var sw = Stopwatch.StartNew();
        for (int i = 1; i < lines.Length; i *= 2)
        {
            sw.Restart();
            for (int j = 0; j < i; ++j)
            {
                ComputeHash(Encoding.UTF8.GetBytes(lines[j]));
            }
            sw.Stop();
            var time1 = sw.ElapsedMilliseconds;
            
            sw.Restart();
            for (int j = 0; j < i; ++j)
            {
                ComputeHash(Encoding.UTF8.GetBytes(lines[j]));
            }
            sw.Stop();
            var time2 = sw.ElapsedMilliseconds;
            
            sw.Restart();
            for (int j = 0; j < i; ++j)
            {
                ComputeHash(Encoding.UTF8.GetBytes(lines[j]));
            }
            sw.Stop();
            var time3 = sw.ElapsedMilliseconds;
            
            var avgTimeMs = (time1 + time2 + time3) / 3;
            speedResults.Add((i, avgTimeMs));
        }
        
        var outputPath = WriteToCsv(speedResults);
        Console.WriteLine($"[INFO] Speed test succeeded. Results exported to: {outputPath}");
    }
    
    private double CollisionTest()
    {
        int correct = 0, testCount = 0;

        foreach (var file in Directory.GetFiles(InputPairsDir, "RandomPairs*.txt"))
        {
            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                var parts = line.Split(' ', 2);
                if (parts.Length != 2)
                {
                    continue;
                }
                var h1 = ComputeHash(Encoding.UTF8.GetBytes(parts[0]));
                var h2 = ComputeHash(Encoding.UTF8.GetBytes(parts[1]));

                if (!h1.SequenceEqual(h2))
                    correct++;
                testCount++;
            }
        }

        return (correct / (double) testCount) * 100;
    }
    
    private (double minBitPct, double maxBitPct, double avgBitPct, double minHexPct, double maxHexPct, double avgHexPct) AvalancheTest()
    {
        int minDiffBit = 256, maxDiffBit = 0;
        long sumDiffBit = 0;
        int minDiffHex = 256, maxDiffHex = 0;
        long sumDiffHex = 0;
        int lineCount = 0;
        
        foreach (var file in Directory.GetFiles(InputPairsDir, "PairsRandomChar*.txt"))
        {
            foreach (var line in File.ReadLines(file, Encoding.UTF8))
            {
                var parts = line.Split(' ', 2);
                if (parts.Length != 2)
                {
                    continue;
                }
                var h1 = ComputeHash(Encoding.UTF8.GetBytes(parts[0]));
                var h2 = ComputeHash(Encoding.UTF8.GetBytes(parts[1]));


                var bitDiff = comparer.BitDifference(h1, h2);
                //TODO remove
                if (bitDiff < 42)
                {
                    Console.WriteLine("====");
                    Console.WriteLine(parts[0]);
                    Console.WriteLine(parts[1]);

                    Console.WriteLine(Encoding.UTF8.GetString(h1));
                    Console.WriteLine(Encoding.UTF8.GetString(h2));
                    
                }
                minDiffBit = Math.Min(minDiffBit, bitDiff);
                maxDiffBit = Math.Max(maxDiffBit, bitDiff);
                sumDiffBit += bitDiff;

                string hex1 = converter.BytesToHex(h1);
                string hex2 = converter.BytesToHex(h2);

                int hexDiff = comparer.HexDifference(hex1, hex2);
                minDiffHex = Math.Min(minDiffHex, hexDiff);
                maxDiffHex = Math.Max(maxDiffHex, hexDiff);
                sumDiffHex += hexDiff;

                lineCount++;
            }
        }
        Console.WriteLine(lineCount);
        if (lineCount == 0) return (0, 0, 0, 0, 0, 0);

        // Convert to percentages
        double minBitPct = minDiffBit / (double)HashLengthBit * 100;
        double maxBitPct = maxDiffBit / (double)HashLengthBit * 100;
        double avgBitPct = sumDiffBit / (double)(lineCount * HashLengthBit) * 100;

        double minHexPct = minDiffHex / (double)HashLengthHex * 100;
        double maxHexPct = maxDiffHex / (double)HashLengthHex * 100;
        double avgHexPct = sumDiffHex / (double)(lineCount * HashLengthHex) * 100;

        return (minBitPct, maxBitPct, avgBitPct, minHexPct, maxHexPct, avgHexPct);
    }

    private string WriteToCsv(List<(int lines, long avgTimeMs)> speedResults)
    {
        Directory.CreateDirectory(OutputDir);
        string csvPath = Path.Combine(OutputDir, "HashSpeed.csv");
        using var writer = new StreamWriter(csvPath, false);
        writer.WriteLine("Lines;AvgTimeMs"); // header
        foreach (var result in speedResults)
        {
            writer.WriteLine($"{result.lines};{result.avgTimeMs}");
        }

        return csvPath;
    }
}