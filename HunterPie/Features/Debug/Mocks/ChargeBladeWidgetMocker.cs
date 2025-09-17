using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class ChargeBladeWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockChargeBladeWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var config = new ChargeBladeWidgetConfig();
        var viewModel = new ClassViewModel(config)
        {
            InHuntingZone = true,
            Current = MockViewModel()
        };

        return overlay.Register(viewModel);
    }

    private static ChargeBladeViewModel MockViewModel()
    {
        var vm = new ChargeBladeViewModel();

        for (int i = 0; i < 5; i++)
            vm.Phials.Add(new ChargeBladePhialViewModel());

        return vm;
    }
}