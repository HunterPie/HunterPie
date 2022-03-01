using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration
{
    [SettingsGroup("OVERLAY_STRING", "ICON_OVERLAY")]
    public class OverlayConfig : ISettings
    {
        public MonsterWidgetConfig BossesWidget { get; set; } = new();
        public AbnormalityTrayConfig AbnormalityTray { get; set; } = new();
        public DamageMeterWidgetConfig DamageMeterWidget { get; set; } = new();
        public TelemetricsWidgetConfig DebugWidget { get; set; } = new();
        public WirebugWidgetConfig WirebugWidget { get; set; } = new();

        [SettingField("OVERLAY_KEYBINDING_TOGGLE_DESIGN_MODE", requiresRestart: true)]
        public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";

        [SettingField("OVERLAY_HIDE_WHEN_GAME_UNFOCUS_STRING")]
        public Observable<bool> HideWhenUnfocus { get; set; } = false;
    }
}
