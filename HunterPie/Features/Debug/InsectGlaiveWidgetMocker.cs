using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

internal class InsectGlaiveWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockInsectGlaiveWidget)
            return;

        var mockSettings = new ClassWidgetConfig();
        _ = WidgetManager.Register<ClassView, ClassWidgetConfig>(
            new ClassView
            {
                DataContext = new ClassViewModel
                {
                    CurrentSettings = mockSettings,
                    Current = new InsectGlaiveViewModel()
                }
            }
        );
    }
}