using System.Security.Cryptography;
using System.Text;

namespace wx.Infrastructure.Utils;

public static class HashUtils
{
    public static string GenarateHash(int size = 32)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(size);
        return Convert.ToBase64String(randomBytes);
    }

    public static string HashString(string base64)
    {
        using var  sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(base64);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToHexString(hashBytes).ToLowerInvariant();
    }
}
