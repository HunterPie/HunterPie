using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Metrics.View;

namespace HunterPie.Internal.Initializers
{
    internal class DebugWidgetInitializer : IInitializer
    {
        public void Init()
        {
            var config = ClientConfigHelper.GetOverlayConfigFrom(GameProcess.MonsterHunterRise);
            if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_METRICS_WIDGET))
                WidgetManager.Register<TelemetricsView, TelemetricsWidgetConfig>(new TelemetricsView(config.DebugWidget));
        }
    }
}
