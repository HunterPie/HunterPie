using HunterPie.Core.Client;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Settings.Types;
using HunterPie.Core.System.Windows.Registry;
using HunterPie.Domain.Interfaces;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

public class LocalConfigInitializer : IInitializer
{
    private const string KEY_SECRET = "secret";
    private const string CLIENT_ID = "client_id";

    public Task Init()
    {
        InitializeLocalRegistry();
        GenerateSecretKey();
        GenerateClientId();

        return Task.CompletedTask;
    }

    private void InitializeLocalRegistry()
    {
        ILocalRegistry registry;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            registry = new WindowsRegistry();
        else
            throw new NotImplementedException("unsupported OS");

        RegistryConfig.Initialize(registry);
    }

    /// <summary>
    /// Generates a new Secret Key, this will be used SOLELY to keep string values safe in the config.json when using the <seealso cref="Secret"/>
    /// class, so users can share their config.json without the risk of losing whatever was saved in the Secret text box.
    /// </summary>
    private void GenerateSecretKey()
    {
        if (RegistryConfig.Exists(KEY_SECRET))
            return;

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

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

        using RandomNumberGenerator rng = RandomNumberGenerator.Create();

        byte[] bytes = new byte[16];
        rng.GetBytes(bytes);

        string token = BitConverter.ToString(bytes)
            .Replace("-", string.Empty);

        RegistryConfig.Set(CLIENT_ID, token);
    }
}
