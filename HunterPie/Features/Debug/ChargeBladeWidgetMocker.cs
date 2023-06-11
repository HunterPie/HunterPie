using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.Core.Extensions;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using HunterPie.UI.Overlay.Widgets.Classes.Views;

namespace HunterPie.Features.Debug;

internal class ChargeBladeWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        if (!ClientConfig.Config.Development.MockChargeBladeWidget)
            return;

        var view = new ClassView();

        view.ViewModel.CurrentSettings = ClientConfig.Config.Rise.Overlay.ChargeBladeWidget;
        view.ViewModel.InHuntingZone = true;
        view.ViewModel.Current = MockViewModel();

        _ = WidgetManager.Register<ClassView, ClassWidgetConfig>(view);
    }

    private static ChargeBladeViewModel MockViewModel()
    {
        var vm = new ChargeBladeViewModel();

        foreach (int _ in 0..5)
            vm.Phials.Add(new ChargeBladePhialViewModel());

        return vm;
    }
}