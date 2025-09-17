using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class SwitchAxeWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockSwitchAxeWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var viewModel = new ClassViewModel(ClientConfig.Config.Rise.Overlay.SwitchAxeWidget)
        {
            InHuntingZone = true,
            Current = new SwitchAxeViewModel
            {
                MaxChargedTimer = 100.0f,
                ChargedTimer = 50.0f
            }
        };

        return overlay.Register(viewModel);
    }
}