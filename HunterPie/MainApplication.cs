using HunterPie.Core.Analytics;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Process.Internal;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.Controller;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Analytics.Entity;
using HunterPie.Features.Game.Service;
using HunterPie.Internal;
using HunterPie.UI.Main.Navigators;
using HunterPie.Update.UseCase;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace HunterPie;

internal class MainApplication(
    IAnalyticsService analyticsService,
    IUpdateUseCase updateUseCase,
    IRemoteAccountConfigUseCase remoteAccountConfigUseCase,
    RemoteConfigSyncService remoteConfigSyncService,
    NavigatorController navigatorController,
    GameContextController gameContextController,
    AccountController accountController,
    IControllableWatcherService controllableWatcherService) : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public async Task<bool> Start()
    {
#if RELEASE
        bool hasUpdated = await SelfUpdate();

        if (hasUpdated)
            return false;
#endif
        gameContextController.Subscribe();
        remoteConfigSyncService.Start();
        await navigatorController.SetupAsync();
        controllableWatcherService.Start();
        await accountController.SetupAsync();

        return true;
    }

    public async Task SendUiException(Exception exception)
    {
        _logger.Error(exception.ToString());

        await analyticsService.SendAsync(
            analyticsEvent: AnalyticsEvent.FromException(exception, isUiError: true)
        );
    }

    public async Task Restart()
    {
        await remoteAccountConfigUseCase.Upload();

        string executablePath = typeof(MainApplication).Assembly.Location.Replace(".dll", ".exe");
        Process.Start(executablePath);
    }

    private async Task<bool> SelfUpdate()
    {
        bool hasUpdated = await updateUseCase.InvokeAsync();

        if (!hasUpdated)
            return false;

        InitializerManager.Unload();
        await Restart();

        return true;
    }

    public void Dispose()
    {
        ConfigManager.SaveAll();
        AsyncHelper.RunSync(remoteAccountConfigUseCase.Upload);
    }
}