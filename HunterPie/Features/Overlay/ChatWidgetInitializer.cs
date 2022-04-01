using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Chat;

namespace HunterPie.Features.Overlay
{
    internal class ChatWidgetInitializer : IWidgetInitializer
    {
        private IContextHandler _handler;

        public void Load(Context context)
        {
            if (!ClientConfig.Config.Overlay.ChatWidget.Initialize)
                return;

            _handler = new ChatWidgetContextHandler(context);
        }

        public void Unload()
        {
            _handler?.UnhookEvents();
        }
    }
}
