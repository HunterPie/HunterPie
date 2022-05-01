using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using System.Collections.Generic;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("ABNORMALITY_WIDGET", "ICON_STOPWATCH")]
    public class AbnormalityWidgetConfig : IWidgetSettings, ISettings
    {
        [SettingField("ABNORMALITY_TRAY_NAME_STRING")]
        public Observable<string> Name { get; set; } = "Abnormality Tray";

        [SettingField("INITIALIZE_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("ENABLE_WIDGET_STRING")]
        public Observable<bool> Enabled { get; set; } = true;

        [SettingField("HIDE_WHEN_UI_VISIBLE_STRING")]
        public Observable<bool> HideWhenUiOpen { get; set; } = false;

        [SettingField("ABNORMALITY_TRAY_MAX_SIZE_STRING")]
        public Range MaxSize { get; set; } = new Range(300, 1200, 30, 30);

        [SettingField("ABNORMALITY_TRAY_SORT_BY_STRING")]
        public Observable<SortBy> SortByAlgorithm { get; set; } = SortBy.Off;

        [SettingField("WIDGET_OPACITY")]
        public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

        [SettingField("WIDGET_SCALE")]
        public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

        [SettingField("ENABLE_STREAMER_MODE")]
        public Observable<bool> StreamerMode { get; set; } = false;

        [SettingField("ORIENTATION_STRING")]
        public Observable<Orientation> Orientation { get; set; } = Enums.Orientation.Horizontal;
        
        [SettingField("WIDGET_POSITION")]
        public Position Position { get; set; } = new(100, 100);

        public HashSet<string> AllowedAbnormalities { get; set; } = new();
    }
}
