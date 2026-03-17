using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Overlay.Services;
using HunterPie.Features.Overlay.Widgets;
using HunterPie.Features.Plugins.Services;
using HunterPie.Features.Scan.Service;
using HunterPie.Integrations.Discord.Factory;
using HunterPie.Integrations.Discord.Service;
using HunterPie.Integrations.Services;
using HunterPie.Integrations.Services.Exceptions;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.Features.Game.Service;

internal class GameContextController(
    Dispatcher uiDispatcher,
    IProcessWatcherService processWatcherService,
    IGameContextService gameContextService,
    IBackupService backupService,
    IControllableScanService controllableScanService,
    DiscordPresenceFactory discordPresenceFactory,
    OverlayManager overlayManager,
    WidgetInitializers widgetInitializers,
    PluginLoader pluginLoader) : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private bool _isDisposed;
    private Context? _context;

    private CancellationTokenSource? _cancellationTokenSource;
    private DiscordPresenceService? _discordPresenceService;

    public void Subscribe()
    {
        processWatcherService.ProcessStart += OnProcessStart;
        processWatcherService.ProcessExit += OnProcessExit;
    }

    private async void OnProcessStart(object? sender, ProcessEventArgs e)
    {
        _cancellationTokenSource = new CancellationTokenSource();
        _context = gameContextService.Get(e.Game);

        _logger.Debug("Initialized game context");

        await _logger.CatchAndLogAsync(async () =>
        {
            await uiDispatcher.InvokeAsync(() => overlayManager.Setup(_context));

            await ContextInitializers.InitializeAsync(_context);

            await uiDispatcher.InvokeAsync(() => widgetInitializers.InitializeAsync(_context));

            controllableScanService.Start(_cancellationTokenSource.Token);
        });

        _logger.CatchAndLog(() =>
        {
            _discordPresenceService = discordPresenceFactory.Create(_context);
            _discordPresenceService.Start();
        });

        await _logger.CatchAndLogAsync(async () =>
        {
            await backupService.ExecuteAsync(
                gameType: MapFactory.Map<GameProcessType, GameType?>(e.Game.Type)
                          ?? throw new UnsupportedGameException(e.Game.Name)
            );
        });

        await pluginLoader.InitializeAsync(_context);
    }

    private async void OnProcessExit(object? sender, EventArgs e)
    {
        if (_cancellationTokenSource is { })
            await _cancellationTokenSource.CancelAsync();

        _discordPresenceService?.Dispose();

        _context?.Dispose();

        _context = null;

        await uiDispatcher.InvokeAsync(widgetInitializers.Unload);
        await uiDispatcher.InvokeAsync(overlayManager.Dispose);

        _logger.Info("Process has closed");

        SmartEventsTracker.DisposeEvents();
        ContextInitializers.Dispose();

        pluginLoader.Unload();

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            uiDispatcher.Invoke(Application.Current.Shutdown);
    }

    public void Dispose()
    {
        if (_isDisposed)
            return;

        processWatcherService.ProcessStart += OnProcessStart;
        processWatcherService.ProcessExit += OnProcessExit;
        _cancellationTokenSource?.Dispose();
        _isDisposed = true;
    }
}