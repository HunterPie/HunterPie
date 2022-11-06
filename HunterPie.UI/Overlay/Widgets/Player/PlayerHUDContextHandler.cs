using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Widgets.Player.Views;

namespace HunterPie.UI.Overlay.Widgets.Player;
public class PlayerHUDContextHandler : IContextHandler
{

    private readonly PlayerHudView View;

    public PlayerHUDContextHandler()
    {
        View = new PlayerHudView();
        _ = WidgetManager.Register<PlayerHudView, WirebugWidgetConfig>(View);
    }

    public void HookEvents() { }
    public void UnhookEvents() { }
}
