using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Utils;
using HunterPie.Features.Backup.Services;
using HunterPie.Features.Overlay.Services;
using HunterPie.Features.Plugins.Services;
using HunterPie.Features.Scan.Service;
using HunterPie.Features.Statistics.Services;
using HunterPie.Game.Common;
using HunterPie.Integrations.Discord.Service;
using HunterPie.Integrations.Services.Exceptions;
using HunterPie.UI.Architecture.Overlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.Features.Game.Service;

internal class GameIntegrationService(
    IContext context,
    Dispatcher dispatcher,
    OverlayManager overlayManager,
    IControllableScanService scanService,
    DiscordPresenceService discordPresenceService,
    IBackupService backupService,
    PluginLoader pluginLoader,
    QuestTrackerService questTrackerService,
    IGameContextInitializer contextInitializer,
    IWidgetInitializer[] widgetInitializers
) : IDisposable
{
    private readonly ILogger _logger = LoggerFactory.Create();

    private readonly CancellationTokenSource _cancelToken = new();

    public async Task StartAsync()
    {
        await _logger.CatchAndLogAsync(async () =>
        {
            overlayManager.Setup();
            questTrackerService.Setup();

            await dispatcher.InvokeAsync(() =>
            {
                Task.WaitAll(
                    widgetInitializers
                        .Where(it => it.SupportedGames.HasFlag(context.Process.Type))
                        .Select(it => it.LoadAsync())
                );
            });

            await contextInitializer.InitializeAsync();

            scanService.Start(_cancelToken.Token);
        });

        _logger.CatchAndLog(discordPresenceService.Start);

        await _logger.CatchAndLogAsync(async () =>
        {
            await backupService.ExecuteAsync(
                gameType: MapFactory.Map<GameProcessType, GameType?>(context.Process.Type)
                    ?? throw new UnsupportedGameException(context.Process.Name)
            );
        });

        await pluginLoader.LoadAsync();
    }

    public void Dispose()
    {
        _cancelToken.Cancel();

        overlayManager.Dispose();
        discordPresenceService.Dispose();
        pluginLoader.Unload();
        widgetInitializers.DisposeAll();
        questTrackerService.Dispose();

        SmartEventsTracker.DisposeEvents();

        _cancelToken.Dispose();
    }
}