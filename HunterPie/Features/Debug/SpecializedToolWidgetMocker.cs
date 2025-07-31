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
        if (!ClientConfig.Config.Development.MockSpecializedToolWidget)
            return;

        var mockConfig = new SpecializedToolWidgetConfig();

        WidgetManager.Register<SpecializedToolViewV2, SpecializedToolWidgetConfig>(
            new SpecializedToolViewV2(ClientConfig.Config.World.Overlay.PrimarySpecializedToolWidget)
            {
                DataContext = new SpecializedToolViewModelV2
                {
                    Timer = 65,
                    MaxTimer = 100
                }
            }
        );
    }
}