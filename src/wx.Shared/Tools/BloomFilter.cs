using System.Security.Cryptography;

namespace wx.Shared;
public class BloomFilter
{
    private readonly byte[] _bits;
    private readonly int _size;
    private readonly int _hashFunctions;

    public BloomFilter(int size, int hashFunctions)
    {
        _size = size;
        _hashFunctions = hashFunctions;
        _bits = new byte[size / 8 + 1];
    }

    public void Add(string item)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(item);

        for (int i = 0; i < _hashFunctions; i++)
        {
            int position = Math.Abs(MurmurHash3(data, i)) % _size;
            _bits[position / 8] |= (byte)(1 << (position % 8));
        }
    }

    public bool Contains(string item)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(item);

        for (int i = 0; i < _hashFunctions; i++)
        {
            int position = Math.Abs(MurmurHash3(data, i)) % _size;
            if ((_bits[position / 8] & (1 << (position % 8))) == 0)
                return false;
        }
        return true;
    }

    private int MurmurHash3(byte[] data, int seed)
    {
        const uint c1 = 0xcc9e2d51;
        const uint c2 = 0x1b873593;
        int length = data.Length;
        int h1 = seed;
        int roundedEnd = (int)(length & 0xfffffffc);

        for (int i = 0; i < roundedEnd; i += 4)
        {
            uint k1 = BitConverter.ToUInt32(data, i);
            k1 *= c1;
            k1 = (k1 << 15) | (k1 >> 17);
            k1 *= c2;

            h1 ^= (int)k1;
            h1 = (h1 << 13) | (h1 >> 19);
            h1 = (int)(h1 * 5 + 0xe6546b64);
        }

        return h1;
    }
}