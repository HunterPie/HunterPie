using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

internal class LongSwordWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockLongSwordWidget)
            return;

        var mockSettings = new ClassWidgetConfig();
        WidgetManager.Register<ClassView, ClassWidgetConfig>(
            new ClassView
            {
                DataContext = new ClassViewModel
                {
                    CurrentSettings = mockSettings,
                    InHuntingZone = true,
                    Current = MockViewModel()
                }
            }
        );
    }

    private static LongSwordViewModel MockViewModel()
    {
        return new LongSwordViewModel();
    }
}