using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay.Class;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class LongSwordWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockLongSwordWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var mockSettings = new ClassWidgetConfig();

        var viewModel = new ClassViewModel(mockSettings)
        {
            Current = MockViewModel(),
            InHuntingZone = true
        };

        return overlay.Register(viewModel);
    }

    private static LongSwordViewModel MockViewModel()
    {
        return new LongSwordViewModel();
    }
}