using HunterPie.Domain.Interfaces;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

/// <summary>
/// Initializer responsible for fetching and downloading the user's
/// remote config that's been uploaded in past sessions
/// </summary>
internal class RemoteConfigSyncInitializer(
    IRemoteAccountConfigUseCase remoteConfigService,
    RemoteConfigSyncService configSyncService
) : IInitializer
{

    public async Task Init()
    {
        await remoteConfigService.Download();

        configSyncService.Start();
    }
}