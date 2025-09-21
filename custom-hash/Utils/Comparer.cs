namespace custom_hash.Utils;

public class Comparer
{
    public int BitDifference(byte[] a, byte[] b)
    {
        int diff = 0;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            byte x = (byte)(a[i] ^ b[i]);
            for (int j = 0; j < 8; j++)
                if ((x & (1 << j)) != 0) diff++;
        }
        return diff;
    }
}