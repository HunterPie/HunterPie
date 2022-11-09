using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Demos.Sunbreak;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.World;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Activities;
using System;

namespace HunterPie.Features.Overlay;

internal class ActivitiesWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(Context context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.ActivitiesWidget.Initialize)
            return;

        _handler = context switch
        {
            MHRContext ctx => new RiseActivitiesWidgetContextHandler(ctx),
            MHWContext => null,
            MHRSunbreakDemoContext => null,
            _ => throw new NotImplementedException("unreachable")
        };
    }

    public void Unload() => _handler?.UnhookEvents();
}
