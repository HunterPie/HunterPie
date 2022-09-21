using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.Views;

namespace HunterPie.Features.Debug;

internal class SpecializedToolWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        Core.Client.Configuration.OverlayConfig mockConfig = ClientConfig.Config.World.Overlay;

        if (ClientConfig.Config.Development.MockSpecializedToolWidget)
        {
            _ = WidgetManager.Register<SpecializedToolView, SpecializedToolWidgetConfig>(
                new SpecializedToolView(mockConfig.PrimarySpecializedToolWidget)
                {
                    DataContext = new MockSpecializedToolViewModel()
                }
            );
        }
    }
}
