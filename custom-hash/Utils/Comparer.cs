namespace custom_hash.Utils;

public class Comparer
{
    public int BitDifference(byte[] a, byte[] b)
    {
        if (a.Length != b.Length)
        {
            throw new ArgumentException("Byte arrays must have the same length.");
        }

        int diff = 0;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            byte x = (byte)(a[i] ^ b[i]);
            for (int j = 0; j < 8; j++)
                if ((x & (1 << j)) != 0) diff++;
        }
        return diff;
    }
    
    public int HexDifference(string hex1, string hex2)
    {
        if (hex1.Length != hex2.Length)
            throw new ArgumentException("Hex strings must have the same length.");

        int diff = 0;
        for (int i = 0; i < hex1.Length; i++)
        {
            if (hex1[i] != hex2[i])
                diff++;
        }
        return diff;
    }
}