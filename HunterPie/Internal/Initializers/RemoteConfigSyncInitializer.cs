using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.Config;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

/// <summary>
/// Initializer responsible for fetching and downloading the user's
/// remote config that's been uploaded in past sessions
/// </summary>
internal class RemoteConfigSyncInitializer : IInitializer
{
    private readonly RemoteAccountConfigService _remoteConfigService = new();

    public async Task Init()
    {
        await _remoteConfigService.FetchClientConfig();

        new RemoteConfigSyncService(_remoteConfigService)
            .Start();
    }
}
