using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Wirebug;

namespace HunterPie.Features.Overlay
{
    internal class WirebugWidgetInitializer : IWidgetInitializer
    {
        private IContextHandler _handler;

        public void Load(Context context)
        {
            if (!ClientConfig.Config.Overlay.WirebugWidget.Initialize)
                return;

            if (context is MHRContext ctx)
                _handler = new WirebugWidgetContextHandler(ctx);
        }

        public void Unload()
        {
            _handler.UnhookEvents();
        }
    }
}
