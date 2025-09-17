using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class WirebugWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockWirebugWidget;


    public WidgetView Mock(IOverlay overlay)
    {
        var config = new WirebugWidgetConfig();
        var viewModel = new WirebugsViewModel(config);

        return overlay.Register(viewModel);
    }
}