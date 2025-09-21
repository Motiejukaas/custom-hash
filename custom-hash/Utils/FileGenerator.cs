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
    
    public void GenerateFile()
    {
        Console.WriteLine(projectDir);

        Directory.CreateDirectory(outputDir);
        foreach (int len in lengths)
        {
            string filePath = Path.Combine(outputDir, $"Pairs{len}.txt");
            Console.WriteLine($"Generating {pairsPerLength} pairs of length {len} into {filePath}");

            using var writer = new StreamWriter(filePath, false, Encoding.UTF8);

            for (int i = 0; i < pairsPerLength; ++i)
            {
                string s1 = RandomString(len);
                string s2 = RandomString(len);
                writer.WriteLine($"{s1} {s2}");
            }
        }

        Console.WriteLine("All files generated successfully.");
    }

    private string RandomAsciiString(int length)
    {
        var rnd = Random.Shared;
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; ++i)
        {
            sb.Append(AsciiChars[rnd.Next(AsciiChars.Length)]);
        }

        return sb.ToString();
    }
    
    private string RandomString(int length)
    {
        var rnd = Random.Shared;
        var sb = new StringBuilder(length);
        for (int i = 0; i < length; ++i)
        {
            sb.Append(AllowedChars[rnd.Next(AllowedChars.Length)]);
        }

        return sb.ToString();
    }
}