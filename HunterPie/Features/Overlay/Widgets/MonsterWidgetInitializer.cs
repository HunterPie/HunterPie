using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Monster;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class MonsterWidgetInitializer : IWidgetInitializer
{
    private readonly IOverlay _overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise |
        GameProcessType.MonsterHunterWorld |
        GameProcessType.MonsterHunterWilds;

    public MonsterWidgetInitializer(IOverlay overlay)
    {
        _overlay = overlay;
    }

    public Task LoadAsync(IContext context)
    {
        MonsterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            (config) => config.BossesWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        var viewModel = new MonstersViewModel(
            settings: config
        );

        _handler = new MonsterWidgetContextHandler(
            context: context,
            viewModel: viewModel,
            config: config
        );

        _view = _overlay.Register(viewModel);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}