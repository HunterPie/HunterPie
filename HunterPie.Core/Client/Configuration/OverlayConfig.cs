using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration
{
    [SettingsGroup("OVERLAY_STRING", "ICON_OVERLAY")]
    public class OverlayConfig : ISettings
    {
        public MonsterWidgetConfig BossesWidget { get; set; } = new MonsterWidgetConfig();
        public AbnormalityWidgetConfig AbnormalityWidget { get; set; } = new AbnormalityWidgetConfig();
        public DamageMeterWidgetConfig DamageMeterWidget { get; set; } = new DamageMeterWidgetConfig();
        public TelemetricsWidgetConfig DebugWidget { get; set; } = new TelemetricsWidgetConfig();

        [SettingField("OVERLAY_KEYBINDING_TOGGLE_DESIGN_MODE", requiresRestart: true)]
        public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";
    }
}
