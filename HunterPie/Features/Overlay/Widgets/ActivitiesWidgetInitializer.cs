using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.Controllers;
using HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.World.Controllers;
using HunterPie.UI.Overlay.Widgets.Activities.World.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HunterPie.Features.Overlay.Widgets;

internal class ActivitiesWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterWilds |
        GameProcessType.MonsterHunterRise |
        GameProcessType.MonsterHunterWorld;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.ActivitiesWidget.Initialize)
            return Task.CompletedTask;

        var viewModel = new ActivitiesViewModel(config.ActivitiesWidget);

        _handler = context switch
        {
            MHRContext ctx => new MHRiseActivitiesController(
                mainDispatcher: DependencyContainer.Get<Dispatcher>(),
                context: ctx,
                viewModel: viewModel,
                activities: DependencyContainer.Get<MHRiseActivitiesViewModel>()
            ),
            MHWContext ctx => new MHWorldActivitiesController(
                context: ctx,
                viewModel: viewModel,
                activities: DependencyContainer.Get<MHWorldActivitiesViewModel>()
            ),
            MHWildsContext ctx => new MHWildsActivitiesController(
                context: ctx,
                viewModel: viewModel,
                activities: DependencyContainer.Get<MHWildsActivitiesViewModel>(),
                dispatcher: DependencyContainer.Get<Dispatcher>()
            ),
            _ => throw new NotImplementedException("unreachable")
        };
        _view = _overlay.Register(viewModel);
        _handler?.HookEvents();
        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _overlay.Unregister(_view);
        _handler = null;
    }
}