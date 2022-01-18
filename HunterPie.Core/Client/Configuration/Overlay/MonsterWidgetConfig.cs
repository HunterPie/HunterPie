using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("MONSTER_WIDGET", "ICON_SKULL")]
    public class MonsterWidgetConfig : IWidgetSettings, ISettings
    {
        [SettingField("INITIALIZE_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("ENABLE_WIDGET_STRING")]
        public Observable<bool> Enabled { get; set; } = true;

        [SettingField("WIDGET_POSITION")]
        public Position Position { get; set; } = new(100, 100);

        [SettingField("WIDGET_OPACITY")]
        public Range Opacity { get; set; } = new() { Current = 1, Max = 1, Min = 0, Step = 0.1 };

        [SettingField("WIDGET_SCALE")]
        public Range Scale { get; set; } = new() { Current = 1, Max = 2, Min = 0, Step = 0.1 };

        [SettingField("ENABLE_STREAMER_MODE")]
        public Observable<bool> StreamerMode { get; set; } = false;

        [SettingField("MONSTER_WIDGET_MAX_WIDTH_STRING")]
        public Range MaxWidth { get; set; } = new(600, 1000, 200, 1);

        [SettingField("MONSTER_WIDGET_AUTO_HIDE_AILMENTS_STRING")]
        public Observable<bool> AutoHideAilments { get; set; } = false;

        [SettingField("MONSTER_WIDGET_AUTO_HIDE_AILMENTS_DELAY_STRING")]
        public Range AutoHideAilmentsDelay { get; set; } = new(15, 60, 1, 1);

        [SettingField("MONSTER_WIDGET_AUTO_HIDE_PARTS_STRING")]
        public Observable<bool> AutoHideParts { get; set; } = false;

        [SettingField("MONSTER_WIDGET_AUTO_HIDE_PARTS_DELAY_STRING")]
        public Range AutoHidePartsDelay { get; set; } = new(15, 60, 1, 1);
    }
}
