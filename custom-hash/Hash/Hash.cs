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

    
    // === AI versions ===
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

    // ---------- HashV04: ARX-based 256-bit hash (experimental, nonstandard) ----------
    public byte[] HashV11(byte[] input)
    {
        // Internal state: eight 32-bit words = 8 * 32 = 256 bits
        uint[] state = new uint[8];

        // Initialization constants (nothing-up-my-sleeve bytes derived from fractional parts of primes)
        state[0] = 0x243F6A88u;
        state[1] = 0x85A308D3u;
        state[2] = 0x13198A2Eu;
        state[3] = 0x03707344u;
        state[4] = 0xA4093822u;
        state[5] = 0x299F31D0u;
        state[6] = 0x082EFA98u;
        state[7] = 0xEC4E6C89u;

        // Pad the input: append 0x80, zeros, and 64-bit length in bits (little-endian)
        byte[] padded = PadMessage(input, 32);

        // Process each 32-byte block
        for (int offset = 0; offset < padded.Length; offset += 32)
        {
            // Parse block into eight 32-bit words (little-endian)
            uint[] m = new uint[8];
            for (int i = 0; i < 8; i++)
                m[i] = BitConverter.ToUInt32(padded, offset + i * 4);

            // Inject message into state (mix): add message words into state
            for (int i = 0; i < 8; i++)
                unchecked { state[i] += m[i]; }

            // Apply permutation rounds (12 rounds recommended)
            const int ROUNDS = 12;
            for (int r = 0; r < ROUNDS; r++)
            {
                // Column round (four quarter-rounds operating on columns)
                G(ref state[0], ref state[4], ref state[1], ref state[5], m[(r + 0) & 7], m[(r + 1) & 7]);
                G(ref state[2], ref state[6], ref state[3], ref state[7], m[(r + 2) & 7], m[(r + 3) & 7]);

                // Diagonal round
                G(ref state[0], ref state[5], ref state[2], ref state[7], m[(r + 4) & 7], m[(r + 5) & 7]);
                G(ref state[4], ref state[1], ref state[6], ref state[3], m[(r + 6) & 7], m[(r + 7) & 7]);

                // Extra mixing: xor with rotated constants to break symmetries
                unchecked
                {
                    state[0] ^= RotateLeft(state[3], 7);
                    state[1] ^= RotateLeft(state[4], 11);
                    state[2] ^= RotateLeft(state[5], 13);
                    state[3] ^= RotateLeft(state[6], 17);
                    state[4] ^= RotateLeft(state[7], 19);
                    state[5] ^= RotateLeft(state[0], 23);
                    state[6] ^= RotateLeft(state[1], 3);
                    state[7] ^= RotateLeft(state[2], 5);
                }
            }

            // Feed-forward (XOR the permuted state with the original message words for diffusion)
            for (int i = 0; i < 8; i++)
                unchecked { state[i] ^= m[i]; }
        }

        // Finalization: run a few extra permutation rounds on state alone
        for (int r = 0; r < 8; r++)
        {
            // simple mixing rounds without message
            G(ref state[0], ref state[4], ref state[1], ref state[5], 0x9E3779B9u ^ (uint)r, 0x7F4A7C15u ^ (uint)(r * 3));
            G(ref state[2], ref state[6], ref state[3], ref state[7], 0xC13FA9A9u ^ (uint)r, 0x91E10DA5u ^ (uint)(r * 7));
        }

        // Produce 32-byte hash: concatenate state words little-endian
        byte[] output = new byte[32];
        for (int i = 0; i < 8; i++)
        {
            byte[] w = BitConverter.GetBytes(state[i]); // little-endian on .NET
            Array.Copy(w, 0, output, i * 4, 4);
        }

        return output;
    }

    // === Helper methods ===
    
    // ARX quarter-round (inspired by ChaCha G but different constants) 
    private static void G(ref uint a, ref uint b, ref uint c, ref uint d, uint x, uint y)
    {
        unchecked
        {
            a = a + b + x;
            d ^= a;
            d = RotateLeft(d, 16);

            c = c + d;
            b ^= c;
            b = RotateLeft(b, 12);

            a = a + b + y;
            d ^= a;
            d = RotateLeft(d, 8);

            c = c + d;
            b ^= c;
            b = RotateLeft(b, 7);
        }
    }

    // Rotate left for 32-bit words
    private static uint RotateLeft(uint value, int count)
    {
        count &= 31;
        return (value << count) | (value >> (32 - count));
    }

    // Message padding (like SHA-style, blockSize bytes)
    private static byte[] PadMessage(byte[] input, int blockSizeBytes)
    {
        // append 0x80, then zeros, then 64-bit length in bits (little-endian)
        ulong bitLen = (ulong)input.Length * 8UL;

        int totalLen = input.Length + 1 + 8; // at least one 0x80 and 8 bytes length
        int pad = (blockSizeBytes - (totalLen % blockSizeBytes)) % blockSizeBytes;
        int finalLen = totalLen + pad;

        byte[] padded = new byte[finalLen];
        Array.Copy(input, 0, padded, 0, input.Length);
        padded[input.Length] = 0x80;
        // zeros already present by default
        byte[] lenBytes = BitConverter.GetBytes(bitLen); // little-endian
        Array.Copy(lenBytes, 0, padded, finalLen - 8, 8);

        return padded;
    }

    // Circular left rotate whole array by given number of bytes.
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