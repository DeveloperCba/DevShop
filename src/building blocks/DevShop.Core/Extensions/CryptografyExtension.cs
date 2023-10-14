using System.Security.Cryptography;
using System.Text;

namespace DevShop.Core.Extensions;

public static class CryptografyExtension
{
    public static string CryptoDescrypto(string text, string key, bool ehCrypto)
    {
        if (ehCrypto)
            return Encrypt(text,key);

        return Decrypt(text, key);
    }

    public static string Encrypt(string text, string encryptionKey)
    {
        var textBytes = Encoding.Unicode.GetBytes(text);
        using (var encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(textBytes, 0, textBytes.Length);
                cryptoStream.Close();
            }
            text = Convert.ToBase64String(memoryStream.ToArray());
        }
        return text;
    }

    public static string Decrypt(string text, string encryptionKey)
    {
        text = text.Replace(" ", "+");
        var cipherBytes = Convert.FromBase64String(text);
        using (Aes encryptor = Aes.Create())
        {
            var pdb = new Rfc2898DeriveBytes(encryptionKey, new byte[]
            {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
            });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                cryptoStream.Close();
            }
            text = Encoding.Unicode.GetString(memoryStream.ToArray());
        }
        return text;
    }
}