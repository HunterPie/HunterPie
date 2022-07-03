using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Logger;

namespace HunterPie.Core.Client
{
    public class ClientConfig
    {
        public const string CONFIG_NAME = "config.json";

        private readonly V3Config _config = new();
        private static ClientConfig _instance;

        public static V3Config Config
        {
            get => _instance._config;
        }

        private ClientConfig()
        {
            _instance = this;
            ConfigManager.Register(CONFIG_NAME, _config);

            Log.Info("Initialized HunterPie Client configuration.");
        }

        internal static void Initialize()
        {
            if (_instance is not null)
                return; 

            _ = new ClientConfig();
        }
    }
}
