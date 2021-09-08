using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration
{
    [SettingsGroup("OVERLAY_STRING", "OVERLAY_STRING_DESC", "Icon")]
    public class OverlayConfig : ISettings
    {
        [SettingField("TEST_STRING", "TEST_STRING_DESC")]
        public string Test { get; set; } = "Hello";
    }
}
