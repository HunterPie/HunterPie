using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("MONSTER_WIDGET", "MONSTER_WIDGET_DESC", "ICON_SKULL")]
    public class MonsterWidgetConfig : IWidgetSettings, ISettings
    {
        [SettingField("A", "B")]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("A", "B")]
        public Observable<bool> Enabled { get; set; } = true;

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
