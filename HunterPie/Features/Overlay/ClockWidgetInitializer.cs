using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Clock;

namespace HunterPie.Features.Overlay;

public class ClockWidgetInitializer : IWidgetInitializer
{

    private IContextHandler? _handler;

    public void Load(IContext context)
    {
        _handler = new ClockWidgetContextHandler(context);
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}