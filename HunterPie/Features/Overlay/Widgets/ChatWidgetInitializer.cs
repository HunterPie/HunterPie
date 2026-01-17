using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Chat;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class ChatWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames => GameProcessType.MonsterHunterRise;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.ChatWidget.Initialize)
            return Task.CompletedTask;

        var viewModel = new ChatViewModel(config.ChatWidget);

        _handler = new ChatWidgetContextHandler(
            context: context,
            viewModel: viewModel
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