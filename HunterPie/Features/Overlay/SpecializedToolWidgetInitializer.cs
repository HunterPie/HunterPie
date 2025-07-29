using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Controllers;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class SpecializedToolWidgetInitializer : IWidgetInitializer
{
    private readonly List<IContextHandler> _handlers = new(2);

    public GameProcessType SupportedGames => GameProcessType.MonsterHunterWorld;

    public Task LoadAsync(IContext context)
    {
        SpecializedToolWidgetConfig[] configs =
        {
            ClientConfigHelper.DeferOverlayConfig(
                game: context.Process.Type,
                deferDelegate: cfg => cfg.PrimarySpecializedToolWidget
            ),
            ClientConfigHelper.DeferOverlayConfig(
                game: context.Process.Type,
                deferDelegate: cfg => cfg.SecondarySpecializedToolWidget
            )
        };

        for (int i = 0; i < configs.Length; i++)
        {
            SpecializedToolWidgetConfig config = configs[i];

            if (!config.Initialize)
                continue;

            IContextHandler controller = CreateControllerByGame(
                context: context,
                index: i,
                configuration: config
            );

            controller.HookEvents();

            _handlers.Add(controller);
        }

        return Task.CompletedTask;
    }

    public void Unload()
    {
        foreach (IContextHandler handler in _handlers)
            handler.UnhookEvents();

        _handlers.Clear();
    }

    private static IContextHandler CreateControllerByGame(
        IContext context,
        int index,
        SpecializedToolWidgetConfig configuration)
    {
        var view = new SpecializedToolViewV2(
            config: configuration
        );

        return context.Game.Player switch
        {
            MHWPlayer player => new SpecializedToolController(
                context: context,
                tool: player.Tools.ElementAtOrDefault(index),
                view: view,
                config: configuration
            ),

            _ => throw new NotImplementedException($"{context.Process.Type} does not support specialized tools")
        };
    }
}