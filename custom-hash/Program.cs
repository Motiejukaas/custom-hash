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

            if (args[0] == "--help" || args[0] == "-h" || args[0] == "/?")
            {
                Console.WriteLine("Usage: dotnet run <hash_algorithm> [input] [--test]");
                Console.WriteLine();
                Console.WriteLine("Arguments:");
                Console.WriteLine("  <hash_algorithm>  Name of the hash algorithm to use.");
                Console.WriteLine("  [input]           Optional. Text string or path to a file to hash.");
                Console.WriteLine("  --test            Optional. Run the automated test suite.");
                Console.WriteLine("                    When using --test, no input string or file is required.");
                Console.WriteLine();
                Console.WriteLine("Available hash algorithms:");
                Console.WriteLine("  hashv0.1  - Custom Hash version 01");
                Console.WriteLine("  hashv0.2  - Custom Hash version 02");
                Console.WriteLine("  hashv0.3  - Custom Hash version 02");
                Console.WriteLine("  hashv1.0  - Custom Hash version 02");
                Console.WriteLine("  hashv1.1  - Custom Hash version 02");
                Console.WriteLine("  sha256   - Standard SHA-256");
                Console.WriteLine("  sha1   - Standard SHA-1");
                Console.WriteLine("  md5      - Standard MD5");
                Console.WriteLine();
                Console.WriteLine("Examples:");
                Console.WriteLine("  dotnet run sha256 \"Hello World\"");
                Console.WriteLine("  dotnet run hashv01 input.txt");
                Console.WriteLine("  dotnet run md5 --test   # Runs automated tests, no input needed");
                return;
            }

            
            string algoName = args[0].ToLower();

            Func<byte[], byte[]> hashFunc = algoName switch
            {
                "hashv0.1" => new Hash.Hash().HashV01,
                "hashv0.2" => new Hash.Hash().HashV02,
                "hashv0.3" => new Hash.Hash().HashV03,
                "hashv1.0" => new Hash.Hash().HashV10,
                "hashv1.1" => new Hash.Hash().HashV11,
                "sha256" => System.Security.Cryptography.SHA256.Create().ComputeHash,
                "sha1" => System.Security.Cryptography.SHA1.Create().ComputeHash,
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
                
                string exeDir = AppContext.BaseDirectory;
                string projectDir = Directory.GetParent(exeDir)
                    .Parent
                    .Parent
                    .Parent
                    .FullName;
                string inputDir = Path.Combine(projectDir, "Data", secondArg);
                if (File.Exists(inputDir))
                {
                    // Read file as bytes
                    inputBytes = File.ReadAllBytes(inputDir);
                    Console.WriteLine($"[INFO] Read file: {secondArg} ({inputBytes.Length} bytes)");
                }
                else
                {
                    // Treat the arg as direct input text
                    inputString = secondArg;
                    Console.WriteLine("[INFO] Command-line string input.");
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

            // Print raw input
            Console.WriteLine();
            Console.WriteLine("=== Input Bytes ===");
            Console.WriteLine("Decimal : " + string.Join(", ", inputBytes));
            Console.WriteLine("Hex     : " + BitConverter.ToString(inputBytes));

            try
            {
                string asString = Encoding.UTF8.GetString(inputBytes);
                Console.WriteLine("String  : " + asString);
            }
            catch
            {
                Console.WriteLine("String  : [unprintable UTF-8 data]");
            }
            Console.WriteLine();

            Converter converter = new Converter();

            var hash = hashFunc(inputBytes);
            Console.WriteLine();
            Console.WriteLine($"=== {algoName} ===");
            Console.WriteLine("Decimal : " + string.Join(", ", hash));
            Console.WriteLine("Hex     : " + BitConverter.ToString(hash));

            try
            {
                string asString = Encoding.UTF8.GetString(hash);
                Console.WriteLine("String  : " + asString);
            }
            catch
            {
                Console.WriteLine("String  : [unprintable UTF-8 data]");
            }
            Console.WriteLine(converter.BytesToHex(hash));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"[ERROR]: {ex.Message}");
        }
    }
}