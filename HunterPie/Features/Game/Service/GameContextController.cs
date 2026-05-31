using HunterPie.Core.Client;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.DI;
using HunterPie.Integrations.Services;
using System;
using System.Windows;
using System.Windows.Threading;

namespace HunterPie.Features.Game.Service;

internal class GameContextController(
    IDependencyRegistry registry,
    Dispatcher uiDispatcher,
    IProcessWatcherService processWatcherService,
    IGameContextFactory gameContextFactory
) : IDisposable
{
    private IScopedDependencyRegistry? _scopedRegistry = null;
    private readonly ILogger _logger = LoggerFactory.Create();

    public void Subscribe()
    {
        processWatcherService.ProcessStart += OnProcessStart;
        processWatcherService.ProcessExit += OnProcessExit;
    }

    private async void OnProcessStart(object? sender, ProcessEventArgs e)
    {
        Context ctx = gameContextFactory.Create(e.Game);

        _scopedRegistry = registry.NewScope();

        _scopedRegistry
            .WithSingle<IContext>((_) => ctx)
            .WithSingle((_) => _scopedRegistry);

        _logger.Debug("Initialized game context");

        DependencyProvider.LoadScopedModules(_scopedRegistry);

        GameIntegrationService integrationService = _scopedRegistry.Get<GameIntegrationService>();

        await integrationService.StartAsync();
    }

    private async void OnProcessExit(object? sender, EventArgs e)
    {
        _logger.Info("Process has closed");

        _scopedRegistry?.Dispose();
        _scopedRegistry = null;

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            uiDispatcher.Invoke(Application.Current.Shutdown);
    }

    public void Dispose()
    {
        processWatcherService.ProcessStart -= OnProcessStart;
        processWatcherService.ProcessExit -= OnProcessExit;
    }
}