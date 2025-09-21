using System.Text;
using custom_hash.Utils;

namespace custom_hash.Tests;

public class Test
{
    void RunTests()
    {
        Hash.Hash hash = new Hash.Hash();
        Converter converter = new Converter();
        Comparer comparer = new Comparer();
        
        Console.WriteLine("=== Hash1 Test Suite ===");
        
        // 1. Fixed length
        string hEmpty = converter.BytesToHex(hash.Hash1(Array.Empty<byte>()));
        Console.WriteLine($"Empty input length: {hEmpty.Length} hex chars (should be 64)");

        // 2. Determinism
        string h1 = converter.BytesToHex(hash.Hash1(Encoding.UTF8.GetBytes("test")));
        string h2 = converter.BytesToHex(hash.Hash1(Encoding.UTF8.GetBytes("test")));
        Console.WriteLine($"Determinism check: {(h1 == h2 ? "PASS" : "FAIL")}");

        // 3. Avalanche effect (bit difference)
        string orig = converter.BytesToHex(hash.Hash1(Encoding.UTF8.GetBytes("Avalanche")));
        string mod  = converter.BytesToHex(hash.Hash1(Encoding.UTF8.GetBytes("avalanche"))); // one letter changed

        int bitDiff = comparer.BitDifference(converter.HexToBytes(orig), converter.HexToBytes(mod));
        Console.WriteLine($"Avalanche bit difference: {bitDiff} / 256 bits");

        // 4. Collision quick check (small sample)
        var set = new System.Collections.Generic.HashSet<string>();
        bool collision = false;
        for (int i = 0; i < 1000; i++)
        {
            string s = "X" + i;
            string h = converter.BytesToHex(hash.Hash1(Encoding.UTF8.GetBytes(s)));
            if (!set.Add(h)) { collision = true; break; }
        }
        Console.WriteLine($"Collision check on 1000 inputs: {(collision ? "FAIL" : "PASS")}");

        Console.WriteLine("=== End of Tests ===");
    }
}