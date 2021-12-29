using HunterPie.Core.Domain.Features;
using HunterPie.Domain.Constants;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Metrics.View;

namespace HunterPie.Internal.Intializers
{
    internal class DebugWidgetInitializer : IInitializer
    {
        public void Init()
        {
            if (FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_METRICS_WIDGET))
                WidgetManager.Register(new TelemetricsView());
        }
    }
}
