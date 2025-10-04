using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class DualBladesWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockDualBladesWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new DualBladesWidgetConfig();
        var viewModel = new ClassViewModel(config)
        {
            Current = MockViewModel(),
            InHuntingZone = true
        };

        return overlay.Register(viewModel);
    }

    private static DualBladesViewModel MockViewModel()
    {
        var vm = new DualBladesViewModel();

        return vm;
    }
}