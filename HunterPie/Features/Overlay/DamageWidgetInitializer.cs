using HunterPie.Core.Client;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Damage;

namespace HunterPie.Features.Overlay
{
    internal class DamageWidgetInitializer : IWidgetInitializer
    {
        IContextHandler _handler;

        public void Load(Context context)
        {
            var config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

            if (!config.DamageMeterWidget.Initialize)
                return;

            if (context is MHRContext && !FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_RISE_DAMAGE_METER))
                return;

            _handler = new DamageMeterWidgetContextHandler(context);
        }

        public void Unload()
        {
            _handler?.UnhookEvents();
        }
    }
}
