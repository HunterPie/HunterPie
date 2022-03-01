using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Converters;
using HunterPie.Core.Logger;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography;

namespace HunterPie.Core.Settings.Types
{
    [JsonConverter(typeof(SecretConverter))]
    public class Secret : Bindable
    {
        private string _value;

        [JsonIgnore]
        public string Value
        {
            get => _value;
            set { SetValue(ref _value, value); }
        }

        public string EncryptedValue
        {
            get => Encrypt(Value);
            set
            {
                Value = Decrypt(value);
            }
        }

        private static string Encrypt(string value)
        {
            byte[] iv = new byte[16];
            byte[] data;

            using (Aes aes = Aes.Create())
            {
                aes.Key = (byte[])RegistryConfig.Get("secret");
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(value);
                        }

                        data = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(data);
        }

        private static string Decrypt(string value)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(value);

                using Aes aes = Aes.Create();
                aes.Key = (byte[])RegistryConfig.Get("secret");
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream memoryStream = new MemoryStream(buffer);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                using StreamReader streamReader = new StreamReader(cryptoStream);

                return streamReader.ReadToEnd();
            } catch(Exception err) { Log.Error(err.ToString()); }
            
            return "";
        }

        public override string ToString()
        {
            return Value;
        }

        public static implicit operator string(Secret self) => self.Value;
    }
}
