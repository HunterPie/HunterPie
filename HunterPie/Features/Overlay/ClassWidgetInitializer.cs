using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes;

namespace HunterPie.Features.Overlay;
internal class ClassWidgetInitializer : IWidgetInitializer
{
    private IContextHandler? _handler;

    public void Load(IContext context)
    {
        _handler = new ClassWidgetContextHandler(context);
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}