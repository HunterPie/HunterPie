using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("TELEMETRICS_WIDGET", "ICON_BUG", FeatureFlags.FEATURE_METRICS_WIDGET)]
    public class TelemetricsWidgetConfig : IWidgetSettings, ISettings
    {
        [SettingField("A", "B")]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("A", "B")]
        public Observable<bool> Enabled { get; set; } = true;

        [SettingField("HIDE_WHEN_UI_VISIBLE_STRING")]
        public Observable<bool> HideWhenUiOpen { get; set; } = false;

        [SettingField("A", "B")]
        public Position Position { get; set; } = new(100, 100);

        [SettingField("A", "B")]
        public Range Opacity { get; set; } = new() { Current = 1, Max = 1, Min = 0, Step = 0.1 };

        [SettingField("A", "B")]
        public Range Scale { get; set; } = new() { Current = 1, Max = 2, Min = 0, Step = 0.1 };

        [SettingField("A", "B")]
        public Observable<bool> StreamerMode { get; set; } = false;
    }
}
