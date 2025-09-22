using System.Text;

namespace custom_hash.Utils;

public class FileGenerator
{ 
    private static readonly char[] AsciiChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:',.<>?/`~".ToCharArray();
    
    private static readonly char[] AllowedChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:',.<>?/`~ąčęėįšųūžĄČĘĖĮŠŲŪŽ".ToCharArray();
    
    private static string exeDir = AppContext.BaseDirectory;
    private static string projectDir = Directory.GetParent(exeDir)
        .Parent
        .Parent
        .Parent
        .FullName;
    private static string outputDir = Path.Combine(projectDir, "Data", "RandomPairs");
    
    private readonly int[] lengths;
    private readonly int pairsPerLength;
    
    public FileGenerator(int[] lengths, int pairsPerLength)
    {
        this.lengths = lengths;
        this.pairsPerLength = pairsPerLength;
    }
    
    public void GenerateFiles()
    {
        Console.WriteLine(projectDir);

        Directory.CreateDirectory(outputDir);
        
        GenerateRandomPairs();
        GeneratePairsRandomChar();
        
        Console.WriteLine("[ OK ] All files generated successfully.");
    }

    private void GenerateRandomPairs()
    {
        foreach (int len in lengths)
        {
            string filePath = Path.Combine(outputDir, $"RandomPairs{len}.txt");
            
            if (File.Exists(filePath))
            {
                Console.WriteLine($"[SKIP] {filePath} already exists.");
                continue;
            }
            
            Console.WriteLine($"[INFO] Generating {pairsPerLength} pairs of length {len} into {filePath}");

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            for (int i = 0; i < pairsPerLength; ++i)
            {
                string s1 = RandomString(len);
                string s2 = RandomString(len);
                writer.WriteLine($"{s1} {s2}");
            }
        }
    }

    private void GeneratePairsRandomChar()
    {
        var rnd  = new Random();
        foreach (int len in lengths)
        {
            if (len == 1)
            {
                continue;
            }
            
            string filePath = Path.Combine(outputDir, $"PairsRandomChar{len}.txt");
            
            if (File.Exists(filePath))
            {
                Console.WriteLine($"[SKIP] {filePath} already exists.");
                continue;
            }
            
            Console.WriteLine($"[INFO] Generating {pairsPerLength} pairs of length {len} into {filePath}");

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);
            for (int i = 0; i < pairsPerLength; ++i)
            {
                string s1 = RandomString(len);
                char[] s2CharArr = s1.ToCharArray();

                s2CharArr[rnd.Next(s2CharArr.Length)] = AllowedChars[rnd.Next(AllowedChars.Length)];             
                writer.WriteLine($"{s1} {new string(s2CharArr)}");
            }
        }
    }
    
    private string RandomAsciiString(int length)
    {
        var rnd = new Random();
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; ++i)
        {
            sb.Append(AsciiChars[rnd.Next(AsciiChars.Length)]);
        }

        return sb.ToString();
    }
    
    private string RandomString(int length)
    {
        var rnd = new Random();
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; ++i)
        {
            sb.Append(AllowedChars[rnd.Next(AllowedChars.Length)]);
        }

        return sb.ToString();
    }
}