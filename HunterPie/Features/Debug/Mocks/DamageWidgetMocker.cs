using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

namespace HunterPie.Features.Debug.Mocks;

internal class DamageWidgetMocker : IWidgetMocker
{
    public Observable<bool> Setting => ClientConfig.Config.Development.MockDamageWidget;

    public WidgetView Mock(IOverlay overlay)
    {
        DamageMeterWidgetConfig config = new();
        var viewModel = new MeterViewModelV2(config);

        return overlay.Register(viewModel);
    }
}