using System.Text;
using custom_hash.Tests;
using custom_hash.Utils;

namespace custom_hash;

internal class Program
{
    public static void Main(string[] args)
    {
        int[] lengths = [1, 10, 100, 500, 1000];
        int pairsPerLength = 25000;

        try
        {
            string? inputString = null;
            byte[]? inputBytes = null;
            
            if (args.Length == 0)
            {
                throw new ArgumentException("Please provide the hash name!");
            }
            
            string algoName = args[0].ToLower();

            Func<byte[], byte[]> hashFunc = algoName switch
            {
                "hashv01" => new Hash.Hash().HashV01,
                "hashv02" => new Hash.Hash().HashV02,
                "sha256" => System.Security.Cryptography.SHA256.Create().ComputeHash,
                "md5" => System.Security.Cryptography.MD5.Create().ComputeHash,
                _ => throw new ArgumentException($"Unknown algorithm: {algoName}")
            };
            
            int hashLengthBytes = hashFunc(Array.Empty<byte>()).Length;
            
            if (args is [_, "--test", ..])
            {
                FileGenerator fileGenerator = new FileGenerator(lengths, pairsPerLength);
                fileGenerator.GenerateFiles();
                
                var test = new Test(hashFunc, hashLengthBytes);
                test.RunTests();

                return;
            }

            if (args.Length >= 2)
            {
                string secondArg = args[1];
                if (File.Exists(secondArg))
                {
                    // Read file as bytes
                    inputBytes = File.ReadAllBytes(secondArg);
                    Console.WriteLine($"[INFO] Read file: {secondArg} ({inputBytes.Length} bytes)");
                }
                else
                {
                    // Treat the arg as direct input text
                    inputString = secondArg;
                    Console.WriteLine("[INFO] Using command-line string input.");
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

            Console.WriteLine(Encoding.UTF8.GetString(inputBytes));
            Converter converter = new Converter();

            string hex = converter.BytesToHex(hashFunc(inputBytes));
            Console.WriteLine();
            Console.WriteLine("Hash1 (256-bit, 64 hex chars):");
            Console.WriteLine(hex);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR]: {ex.Message}");
        }
    }
}