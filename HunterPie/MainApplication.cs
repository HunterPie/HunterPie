using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Internal;
using HunterPie.Update.UseCase;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie;

internal class MainApplication
{
    private readonly IAccountUseCase _accountUseCase;
    private readonly IUpdateUseCase _updateUseCase;
    private readonly IRemoteAccountConfigUseCase _remoteAccountConfigUseCase;
    private readonly RemoteConfigSyncService _remoteConfigSyncService;

    public MainApplication(
        IAccountUseCase accountUseCase,
        IUpdateUseCase updateUseCase,
        IRemoteAccountConfigUseCase remoteAccountConfigUseCase,
        RemoteConfigSyncService remoteConfigSyncService)
    {
        _accountUseCase = accountUseCase;
        _updateUseCase = updateUseCase;
        _remoteAccountConfigUseCase = remoteAccountConfigUseCase;
        _remoteConfigSyncService = remoteConfigSyncService;
    }

    public async Task Start()
    {
#if RELEASE
        await SelfUpdate();
#endif
        _remoteConfigSyncService.Start();
    }

    public async Task Restart()
    {
        await _remoteAccountConfigUseCase.Upload();

        string executablePath = typeof(MainApplication).Assembly.Location.Replace(".dll", ".exe");
        Process.Start(executablePath);
    }

    private async Task SelfUpdate()
    {
        bool hasUpdated = await _updateUseCase.InvokeAsync();

        if (hasUpdated)
        {
            InitializerManager.Unload();
            await Restart();
            return;
        }
    }
}