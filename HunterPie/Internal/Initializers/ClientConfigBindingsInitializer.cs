using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Initializers
{
    internal class ClientConfigBindingsInitializer : IInitializer
    {

        public void Init()
        {
            ConfigManager.BindAndSaveOnChanges(ClientConfig.CONFIG_NAME, ClientConfig.Config);
        }
    }
}
