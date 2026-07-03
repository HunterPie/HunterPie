using HunterPie.Core.Client;
using HunterPie.Core.Domain.Process.Events;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
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
            .WithSingle((_) => ctx);

        _logger.Debug("Initialized game context");

        _logger.CatchAndLog(() => DependencyProvider.LoadScopedModules(_scopedRegistry));
        _logger.CatchAndLog(async () =>
        {
            await _scopedRegistry.Get<GameIntegrationService>()
                .StartAsync();
        });
    }

    private void OnProcessExit(object? sender, EventArgs e) => uiDispatcher.BeginInvoke(() =>
    {
        _logger.Info("Process has closed");

        if (_scopedRegistry is not null)
        {
            _scopedRegistry.Get<GameIntegrationService>().Dispose();
            _scopedRegistry.Get<Context>().Dispose();

            _scopedRegistry.Dispose();

        }

        _scopedRegistry = null;

        if (ClientConfig.Config.Client.ShouldShutdownOnGameExit)
            uiDispatcher.Invoke(Application.Current.Shutdown);
    });

    public void Dispose()
    {
        processWatcherService.ProcessStart -= OnProcessStart;
        processWatcherService.ProcessExit -= OnProcessExit;

        _scopedRegistry?.Dispose();
        _scopedRegistry = null;
    }
}