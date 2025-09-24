namespace custom_hash.Hash;

public class Hash
{
    private const int HashLengthBytes = 32;

    public byte[] HashV01(byte[] input)
    {
        byte[] randomBytes = [105, 27, 24, 71, 113, 30, 4, 26, 144, 187, 127, 67, 211, 216, 176, 75, 212, 125, 79, 99, 46, 17, 207, 113, 67, 125, 183, 178, 14, 68, 72, 12];
        byte[] hash = new byte[HashLengthBytes];
        
        int minLength = Math.Min(randomBytes.Length, input.Length);

        // XOR the overlapping part
        for (int i = 0; i < minLength; i++)
        {
            hash[i] = (byte)(randomBytes[i] ^ input[i]);
        }

        // Copy remaining randomBytes if input is shorter
        for (int i = minLength; i < randomBytes.Length; i++)
        {
            hash[i] = randomBytes[i];
        }
        
        return hash;
    }
    
    public byte[] HashV02(byte[] input)
    {
        //placeholder
        return input;
    }
}