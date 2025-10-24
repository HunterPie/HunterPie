using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

namespace HunterPie.Features.Debug.Mocks;

internal class AbnormalityWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockAbnormalityWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        var mockSettings = new AbnormalityWidgetConfig();
        var viewModel = new AbnormalityBarViewModel(mockSettings);

        return overlay.Register(viewModel);
    }
}