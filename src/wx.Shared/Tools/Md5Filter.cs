namespace wx.Shared;

public static class Md5Filter
{
    public static bool Exists(string md5)
    {
        var md5s = File.ReadAllLines("md5.txt"); //1000万条数据
        HashSet<string> hashSet = new HashSet<string>(md5s);
        return hashSet.Contains(md5);
    }

    private static int MurmurHash3(byte[] data, int seed)
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
