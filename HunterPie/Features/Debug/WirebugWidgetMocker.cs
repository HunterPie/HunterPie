using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using HunterPie.UI.Overlay.Widgets.Wirebug.Views;

namespace HunterPie.Features.Debug
{
    internal class WirebugWidgetMocker : IWidgetMocker
    {
        public void Mock()
        {
            var mockConfig = ClientConfig.Config.Rise.Overlay;

            if (ClientConfig.Config.Development.MockWirebugWidget)
                WidgetManager.Register<WirebugsView, WirebugWidgetConfig>(
                    new WirebugsView(mockConfig.WirebugWidget)
                    {
                        DataContext = new MockWirebugsViewModel()
                    }
                );
        }
    }
}
