using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

namespace HunterPie.Features.Debug;

internal class DamageWidgetMocker : IWidgetMocker
{
    public void Mock()
    {
        Core.Client.Configuration.OverlayConfig mockConfig = ClientConfig.Config.World.Overlay;

        if (!ClientConfig.Config.Development.MockDamageWidget)
            return;

        WidgetManager.Register<MeterViewV2, DamageMeterWidgetConfig>(
            new MeterViewV2(mockConfig.DamageMeterWidget)
            {
                DataContext = new MockMeterViewModel()
            }
        );
    }
}