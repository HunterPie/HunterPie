using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class ClientConfigBindingsInitializer : IInitializer
{

    public Task Init()
    {
        ConfigManager.BindAndSaveOnChanges(ClientConfig.CONFIG_NAME, ClientConfig.Config);

        return Task.CompletedTask;
    }
}
