using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Player;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class PlayerHudWidgetInitializer(
    IContext context,
    IOverlay overlay,
    PlayerHudWidgetConfig config
) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private PlayerHudWidgetContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames => GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld;

    public Task LoadAsync()
    {
        if (!config.Initialize)
            return Task.CompletedTask;

        var viewModel = new PlayerHudViewModel(config);

        _handler = new PlayerHudWidgetContextHandler(
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