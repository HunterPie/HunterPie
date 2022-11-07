using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Player.ViewModels;
using HunterPie.UI.Overlay.Widgets.Player.Views;

namespace HunterPie.Features.Debug;
internal class PlayerHudWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        _ = WidgetManager.Register<PlayerHudView, WirebugWidgetConfig>(
                new PlayerHudView()
                {
                    DataContext = new MockPlayerHudViewModel()
                }
            );
    }
}
