using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class ChatWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockChatWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new ChatWidgetConfig();
        var viewModel = new ChatViewModel(config);

        return overlay.Register(viewModel);
    }
}