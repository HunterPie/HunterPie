using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using System;
using System.IO;
using System.Security.Cryptography;

namespace HunterPie.Core.Crypto;
public class CryptoService
{
    /// <summary>
    /// Encrypts data using the user's HunterPie installation key.
    /// </summary>
    /// <param name="value">Data to be encrypted</param>
    /// <returns>Encrypted data as a Base64 encoded string</returns>
    public static string Encrypt(string value)
    {
        byte[] iv = new byte[16];
        byte[] data;

        using (var aes = Aes.Create())
        {
            aes.Key = (byte[])RegistryConfig.Get("secret");
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            using (var streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(value);
            }

            data = memoryStream.ToArray();
        }

        return Convert.ToBase64String(data);
    }

    /// <summary>
    /// Decrypts a Base64 encoded encrypted string using the user's installation key.
    /// </summary>
    /// <param name="encrypted">Data to be decrypted</param>
    /// <returns>Decrypted data</returns>
    public static string Decrypt(string encrypted)
    {
        try
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(encrypted);

            using var aes = Aes.Create();
            aes.Key = (byte[])RegistryConfig.Get("secret");
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using var memoryStream = new MemoryStream(buffer);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            using var streamReader = new StreamReader(cryptoStream);

            return streamReader.ReadToEnd();
        }
        catch (Exception err)
        {
            Log.Error(err.ToString());
        }

        return "";
    }
}