using System.Security.Cryptography;
using System.Text;
using DevShop.Core.Comunications;
using NETCore.Encrypt;

namespace DevShop.Core.Helpers;

public static class CryptorHelper
{
    public static string Encrypt(string text, RSAEncryptionPadding padding = null)
    {
        if (string.IsNullOrEmpty(text))
            throw new Exception("Valor para encriptação não deve ser vazio ou nulo.");

        var rsa = EncryptProvider.RSAFromPem(RsaEncryptKeysSettings.EncryptKey);
        var data = rsa.Encrypt(Encoding.ASCII.GetBytes(text), padding ?? RSAEncryptionPadding.OaepSHA1);

        return Convert.ToBase64String(data);
    }

    public static string Decrypt(string text, RSAEncryptionPadding padding = null)
    {
        try
        {
            if (string.IsNullOrEmpty(text))
                return null;

            var rsa = EncryptProvider.RSAFromPem(RsaEncryptKeysSettings.DecryptKey);
            var bytes = Convert.FromBase64String(text);
            var data = rsa.Decrypt(bytes, padding ?? RSAEncryptionPadding.OaepSHA1);

            return Encoding.Default.GetString(data);
        }
        catch
        {
            return null;
        }
    }
}