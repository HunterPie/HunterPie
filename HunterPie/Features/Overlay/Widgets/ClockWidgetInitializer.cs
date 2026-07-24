using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Clock;
using HunterPie.UI.Overlay.Widgets.Clock.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

public class ClockWidgetInitializer(
    IContext context,
    IOverlay overlay,
    OverlayConfig config
) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private ClockWidgetContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public Task LoadAsync()
    {
        if (!config.ClockWidget.Initialize)
            return Task.CompletedTask;

        var viewModel = new ClockViewModel(config.ClockWidget);

        _handler = new ClockWidgetContextHandler(
            context: context,
            viewModel: viewModel
        );

        _view = _overlay.Register(viewModel);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}