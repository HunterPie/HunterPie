using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
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

internal class DamageWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public Task LoadAsync(IContext context)
    {
        DamageMeterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            it => it.DamageMeterWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        var viewModel = new MeterViewModelV2(config);

        _view = _overlay.Register(viewModel);

        _handler = new DamageMeterControllerV2(
            context: context,
            viewModel: viewModel,
            widgetContext: (WidgetContext)_view.DataContext,
            config: config
        );

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}