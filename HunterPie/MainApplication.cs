using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Internal;
using HunterPie.Update;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie;

internal class MainApplication
{
    private readonly AccountController _accountController;
    private readonly IUpdateUseCase _updateUseCase;
    private readonly IRemoteAccountConfigUseCase _remoteAccountConfigUseCase;
    private readonly RemoteConfigSyncService _remoteConfigSyncService;

    public MainApplication(
        AccountController accountController,
        IUpdateUseCase updateUseCase,
        IRemoteAccountConfigUseCase remoteAccountConfigUseCase,
        RemoteConfigSyncService remoteConfigSyncService)
    {
        _accountController = accountController;
        _updateUseCase = updateUseCase;
        _remoteAccountConfigUseCase = remoteAccountConfigUseCase;
        _remoteConfigSyncService = remoteConfigSyncService;
    }

    public async Task Start()
    {
        bool hasUpdated = await _updateUseCase.Invoke();

        if (hasUpdated)
        {
            InitializerManager.Unload();
            await Restart();
            return;
        }

        _remoteConfigSyncService.Start();
    }

    public async Task Restart()
    {
        await _remoteAccountConfigUseCase.Upload();

        string executablePath = typeof(MainApplication).Assembly.Location.Replace(".dll", ".exe");
        Process.Start(executablePath);
    }
}