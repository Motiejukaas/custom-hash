using custom_hash.Utils;

internal class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        
        FileGenerator fileGenerator = new FileGenerator([10, 100, 500, 1000], 25000);
        fileGenerator.GenerateFile();
    }
}