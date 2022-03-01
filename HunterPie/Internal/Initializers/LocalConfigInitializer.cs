using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using System;
using HunterPie.Core.Settings.Types;
using System.Security.Cryptography;

namespace HunterPie.Internal.Initializers
{
    public class LocalConfigInitializer : IInitializer
    {
        const string KEY_SECRET = "secret";
        const string CLIENT_ID = "client_id";
        
        public void Init()
        {
            RegistryConfig.Initialize();
            GenerateSecretKey();
            GenerateClientId();
        }

        /// <summary>
        /// Generates a new Secret Key, this will be used SOLELY to keep string values safe in the config.json when using the <seealso cref="Secret"/>
        /// class, so users can share their config.json without the risk of losing whatever was saved in the Secret text box.
        /// </summary>
        private void GenerateSecretKey()
        {
            if (RegistryConfig.Exists(KEY_SECRET))
                return;

            using RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            
            byte[] token = new byte[16];
            rng.GetBytes(token);

            RegistryConfig.Set(KEY_SECRET, token);
        }

        /// <summary>
        /// Generates a new Client Id for this HunterPie client, this will be used in Http requests to the API
        /// </summary>
        private void GenerateClientId()
        {
            if (RegistryConfig.Exists(CLIENT_ID))
                return;

            using RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            
            byte[] bytes = new byte[16];
            rng.GetBytes(bytes);

            string token = BitConverter.ToString(bytes)
                .Replace("-", string.Empty);

            RegistryConfig.Set(CLIENT_ID, token);
        }
    }
}
