using System.Text;

namespace custom_hash.Utils;

public class Converter
{
    public string BytesToHex(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes) sb.AppendFormat("{0:x2}", b);
        return sb.ToString();
    }
    
    public byte[] HexToBytes(string hex)
    {
        int len = hex.Length / 2;
        byte[] result = new byte[len];
        for (int i = 0; i < len; i++)
            result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return result;
    }
}