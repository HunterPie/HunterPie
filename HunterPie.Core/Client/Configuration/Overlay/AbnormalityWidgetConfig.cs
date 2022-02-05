using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

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

        [SettingField("WIDGET_OPACITY")]
        public Range Opacity { get; set; } = new(1, 1, 0.1, 0.1);

        [SettingField("WIDGET_SCALE")]
        public Range Scale { get; set; } = new(1, 2, 0.1, 0.1);

        [SettingField("ENABLE_STREAMER_MODE")]
        public Observable<bool> StreamerMode { get; set; } = false;

        [SettingField("ORIENTATION", "ORIENTATION_DESC")]
        public Observable<Orientation> Orientation { get; set; } = Enums.Orientation.Horizontal;
        
        [SettingField("WIDGET_POSITION")]
        public Position Position { get; set; } = new(100, 100);
    }
}
