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
        
        // Applying sponge function
        if (input.Length > HashLengthBytes)
        {
            int hashIndex = 0;
            for (int inputIndex = HashLengthBytes; inputIndex < input.Length; ++inputIndex)
            {
                hash[hashIndex] = (byte)(hash[hashIndex] ^ (input[inputIndex] << 1));
                if (++hashIndex >= hash.Length)
                {
                    hashIndex = 0;
                }
            }
        }
        return hash;
    }
    
    public byte[] HashV03(byte[] input)
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
        
        // Applying sponge function
        if (input.Length > HashLengthBytes)
        {
            int hashIndex = 0;
            for (int inputIndex = HashLengthBytes; inputIndex < input.Length; ++inputIndex)
            {
                hash = LeftRotateBytesIndividually(hash, 3);
                hash[hashIndex] = (byte)(hash[hashIndex] ^ (input[inputIndex] << 1));
                if (++hashIndex >= hash.Length)
                {
                    hashIndex = 0;
                }
            }
        }
        return hash;
    }

    public byte[] HashV10(byte[] input)
    {
        byte[] randomBytes = [105, 27, 24, 71, 113, 30, 4, 26, 
                              144, 187, 127, 67, 211, 216, 176, 75, 
                              212, 125, 79, 99, 46, 17, 207, 113, 
                              67, 125, 183, 178, 14, 68, 72, 12];

        byte[] hash = new byte[HashLengthBytes];

        int minLength = Math.Min(randomBytes.Length, input.Length);

        // Initial XOR with randomBytes
        for (int i = 0; i < minLength; i++)
        {
            hash[i] = (byte)(randomBytes[i] ^ input[i]);
        }

        // Copy remaining randomBytes if input shorter
        for (int i = minLength; i < randomBytes.Length; i++)
        {
            hash[i] = randomBytes[i];
        }

        // Sponge-like absorption of remaining input
        if (input.Length > HashLengthBytes)
        {
            int hashIndex = 0;
            for (int inputIndex = HashLengthBytes; inputIndex < input.Length; ++inputIndex)
            {
                // Rotate the entire hash left by 1 byte
                hash = LeftRotateWhole(hash, 1);

                // Rotate each byte by (input[inputIndex] % 8)
                hash = LeftRotateBytesIndividually(hash, input[inputIndex] % 8);

                // Mix in data with XOR and addition
                hash[hashIndex] ^= (byte)(input[inputIndex] + 31); // add odd prime
                hash[hashIndex] = (byte)((hash[hashIndex] << 1) | (hash[hashIndex] >> 7)); // extra bit rotate

                if (++hashIndex >= hash.Length)
                {
                    hashIndex = 0;
                }
            }
        }

        return hash;
    }

    /// Circular left rotate whole array by given number of bytes.
    private byte[] LeftRotateWhole(byte[] data, int shift)
    {
        if (data == null || data.Length == 0)
            throw new ArgumentException("Input cannot be null or empty");

        shift %= data.Length;
        if (shift == 0)
            return (byte[])data.Clone();

        byte[] result = new byte[data.Length];
        Array.Copy(data, shift, result, 0, data.Length - shift);
        Array.Copy(data, 0, result, data.Length - shift, shift);

        return result;
    }
    
    private byte[] LeftRotateBytesIndividually(byte[] data, int shift)
    {
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Input cannot be null or empty");
        }

        shift %= 8; // only 8 possible bit rotations inside one byte
        if (shift == 0)
        {
            return (byte[])data.Clone();
        }

        byte[] result = new byte[data.Length];
        for (int i = 0; i < data.Length; ++i)
        {
            result[i] = (byte)((data[i] << shift) | (data[i] >> (8 - shift)));
        }

        return result;
    }
}