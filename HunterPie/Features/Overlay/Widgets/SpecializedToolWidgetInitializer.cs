using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Controllers;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class SpecializedToolWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private readonly List<(IContextHandler, WidgetView)> _handlers = new(2);

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterWorld |
        GameProcessType.MonsterHunterWilds;

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

            var viewModel = new SpecializedToolViewModelV2(
                settings: config
            );
            ISpecializedTool? tool = GetSpecializedToolByGame(context, i);

            if (tool is null)
                continue;

            IContextHandler controller = new SpecializedToolController(
                context: context,
                tool: tool,
                viewModel: viewModel,
                config: config
            );

            controller.HookEvents();
            WidgetView view = _overlay.Register(viewModel);

            _handlers.Add((controller, view));
        }

        return Task.CompletedTask;
    }

    public void Unload()
    {
        foreach ((IContextHandler handler, WidgetView view) in _handlers)
        {
            handler.UnhookEvents();
            _overlay.Unregister(view);
        }

        _handlers.Clear();
    }

    private static ISpecializedTool? GetSpecializedToolByGame(
        IContext context,
        int index)
    {
        return context.Game.Player switch
        {
            MHWPlayer player => player.Tools.ElementAtOrDefault(index),

            MHWildsPlayer player => player.Tools.ElementAtOrDefault(index),

            _ => throw new NotImplementedException($"{context.Process.Type} does not support specialized tools")
        };
    }
}