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
        public ActivitiesWidgetConfig ActivitiesWidget { get; set; } = new();
        public ChatWidgetConfig ChatWidget { get; set; } = new();
        public SpecializedToolWidgetConfig PrimarySpecializedToolWidget { get; set; } = new()
        {
            Position = new(950, 400)
        };
        public SpecializedToolWidgetConfig SecondarySpecializedToolWidget { get; set; } = new()
        {
            Position = new(1000, 375),
            Scale = new(0.9, 2, 0.1, 0.1)
        };
        public Keybinding ToggleDesignMode { get; set; } = "ScrollLock";

        public Observable<bool> HideWhenUnfocus { get; set; } = false;
    }
}
