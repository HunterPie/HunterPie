using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Player;

namespace HunterPie.Features.Overlay;

internal class PlayerHudWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.PlayerHudWidget.Initialize)
            return;

        _handler = new PlayerHudWidgetContextHandler(context);
    }
    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}
