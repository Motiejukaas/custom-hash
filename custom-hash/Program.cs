using System.Text;
using custom_hash.Tests;
using custom_hash.Utils;

namespace custom_hash;

internal class Program
{
    public static void Main(string[] args)
    {
        try
        {
            if (args.Length > 0 && args[0] == "--test")
            {
                FileGenerator fileGenerator = new FileGenerator([1, 10, 100, 500, 1000], 25000);
                fileGenerator.GenerateFiles();
                
                Test test = new Test();
                test.RunTests();
                return;
            }
            
            
            string? inputString = null;
            byte[]? inputBytes = null;

            if (args.Length > 0)
            {
                string firstArg = args[0];
                if (File.Exists(firstArg))
                {
                    // Read file as bytes
                    inputBytes = File.ReadAllBytes(firstArg);
                    Console.WriteLine($"[Info] Read file: {firstArg} ({inputBytes.Length} bytes)");
                }
                else
                {
                    // Treat the arg as direct input text
                    inputString = firstArg;
                    Console.WriteLine("[Info] Using command-line string input.");
                }
            }
            else
            {
                // Interactive prompt
                Console.WriteLine("Enter text to hash (single line). Press Enter when done:");
                inputString = Console.ReadLine() ?? "";
            }

            if (inputBytes == null && inputString != null)
            {
                inputBytes = Encoding.UTF8.GetBytes(inputString);
            }

            // compute hash
            //byte[] hash = Hash1(inputBytes);
            Console.WriteLine(inputBytes);
            Converter converter = new Converter();
            
            string hex = converter.BytesToHex(inputBytes);
            Console.WriteLine();
            Console.WriteLine("Hash1 (256-bit, 64 hex chars):");
            Console.WriteLine(hex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}