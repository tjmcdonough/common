using Airslip.Common.Security.Enums;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Airslip.Common.Security.Implementations;

public static class StringCipher
{
    private const int KeySize = 128;
    private const int KeySizeBytes = KeySize / 8;

    public static string Encrypt(string plainText, string passPhrase, int iterations = 1000, ConversionType conversionType = ConversionType.Base64)
    {
        byte[] keyBytes;
        byte[] saltBytes = GenerateRandomBytes();
            
#pragma warning disable CS0618
        using RijndaelManaged rijAlg = new();
#pragma warning restore CS0618
        using Rfc2898DeriveBytes password = new(passPhrase, saltBytes, iterations);
        keyBytes = password.GetBytes(KeySizeBytes);

        // Create an encryptor to perform the stream transform.
        ICryptoTransform encryptor = rijAlg.CreateEncryptor(keyBytes, rijAlg.IV);

        // Create the streams used for encryption.
        using MemoryStream msEncrypt = new();
        using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write)) {
            using (StreamWriter swEncrypt = new(csEncrypt))
            {
                swEncrypt.Write(plainText);
            }
        }
            
        byte[] encrypted = msEncrypt.ToArray();
        byte[] cipherTextBytes = rijAlg.IV;
        cipherTextBytes = cipherTextBytes.Concat(saltBytes).ToArray();
        cipherTextBytes = cipherTextBytes.Concat(encrypted).ToArray();

        return conversionType switch
        {
            ConversionType.Hex => Convert.ToHexString(cipherTextBytes),
            _ => Convert.ToBase64String(cipherTextBytes)
        };
    }

    public static string EncryptForUrl(string plainText, string passPhrase, int iterations = 1000, ConversionType conversionType = ConversionType.Base64)
    {
        string cipherText = Encrypt(plainText, passPhrase, iterations, conversionType);
        return HttpUtility.UrlEncode(cipherText);
    }

    public static string Decrypt(string cipherText, string passPhrase, int iterations = 1000, ConversionType conversionType = ConversionType.Base64)
    {
        byte[] cipherTextBytesWithIv = conversionType switch
        {
            ConversionType.Hex => Convert.FromHexString(cipherText),
            _ => Convert.FromBase64String(cipherText)
        };
        
        byte[] ivStringBytes = cipherTextBytesWithIv.Take(KeySizeBytes).ToArray();
        byte[] saltBytes = cipherTextBytesWithIv.Skip(KeySizeBytes).Take(KeySizeBytes).ToArray();
        byte[] cipherTextBytes = cipherTextBytesWithIv.Skip(KeySizeBytes * 2).Take(cipherTextBytesWithIv.Length - KeySizeBytes * 2).ToArray();

        using Rfc2898DeriveBytes password = new(passPhrase, saltBytes, iterations);
        byte[] keyBytes = password.GetBytes(KeySizeBytes);
                
#pragma warning disable CS0618
        using RijndaelManaged rijAlg = new();
#pragma warning restore CS0618
        rijAlg.IV = ivStringBytes;

        ICryptoTransform decryptor = rijAlg.CreateDecryptor(keyBytes, rijAlg.IV);

        using MemoryStream msDecrypt = new(cipherTextBytes);
        using CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read);
        using StreamReader srDecrypt = new(csDecrypt);

        string plaintext = srDecrypt.ReadToEnd();

        return plaintext;
    }
        
    private static byte[] GenerateRandomBytes()
    {
        byte[] randomBytes = new byte[KeySizeBytes];
#pragma warning disable CS0618
        using RNGCryptoServiceProvider rngCsp = new();
#pragma warning restore CS0618
        rngCsp.GetBytes(randomBytes);
        return randomBytes;
    }
}