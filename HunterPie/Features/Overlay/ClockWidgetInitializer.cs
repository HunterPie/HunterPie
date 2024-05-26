using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Clock;

namespace HunterPie.Features.Overlay;

public class ClockWidgetInitializer : IWidgetInitializer
{

    private IContextHandler? _handler;

    public void Load(IContext context)
    {
        ClockWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: ProcessManager.Game,
            (cfg) => cfg.ClockWidget
        );

        if (!config.Initialize)
            return;

        _handler = new ClockWidgetContextHandler(
            context: context,
            configuration: config
        );
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}