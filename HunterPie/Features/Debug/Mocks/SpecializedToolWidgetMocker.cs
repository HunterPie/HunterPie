using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class SpecializedToolWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockSpecializedToolWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var mockConfig = new SpecializedToolWidgetConfig();
        var viewModel = new SpecializedToolViewModelV2(mockConfig)
        {
            Timer = 65,
            MaxTimer = 100
        };

        return overlay.Register(viewModel);
    }
}