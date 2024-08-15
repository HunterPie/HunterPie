using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.Integrations.Datasources.MonsterHunterRise;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities;
using System;

namespace HunterPie.Features.Overlay;

internal class ActivitiesWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.ActivitiesWidget.Initialize)
            return;

        _handler = context switch
        {
            MHRContext ctx => new RiseActivitiesWidgetContextHandler(ctx),
            MHWContext ctx => new WorldActivitiesWidgetContextHandler(ctx),
            _ => throw new NotImplementedException("unreachable")
        };

        _handler?.HookEvents();
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}
