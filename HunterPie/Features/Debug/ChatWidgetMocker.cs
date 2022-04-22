using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;
using HunterPie.UI.Overlay.Widgets.Chat.Views;

namespace HunterPie.Features.Debug
{
    internal class ChatWidgetMocker : IWidgetMocker
    {
        public void Mock()
        {
            if (ClientConfig.Config.Debug.MockChatWidget)
                WidgetManager.Register<ChatView, ChatWidgetConfig>(new ChatView()
                {
                    DataContext = new MockChatViewModel()
                });
        }
    }
}
