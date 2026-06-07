using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Damage.Controllers;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class DamageWidgetInitializer(
    IContext context,
    IOverlay overlay,
    OverlayConfig config
) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public Task LoadAsync()
    {
        if (!config.DamageMeterWidget.Initialize)
            return Task.CompletedTask;

        var viewModel = new MeterViewModelV2(config.DamageMeterWidget);

        _view = _overlay.Register(viewModel);

        _handler = new DamageMeterControllerV2(
            context: context,
            viewModel: viewModel,
            widgetContext: (WidgetContext)_view.DataContext,
            config: config.DamageMeterWidget
        );

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}