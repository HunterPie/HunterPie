using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Chat;

namespace HunterPie.Features.Overlay;

internal class ChatWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.ChatWidget.Initialize)
            return;

        if (context is MHRContext ctx)
            _handler = new ChatWidgetContextHandler(ctx);
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}
