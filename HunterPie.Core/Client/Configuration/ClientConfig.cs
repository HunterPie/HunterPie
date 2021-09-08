using HunterPie.Core.Client.Language;
using HunterPie.Core.Client.Themes;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration
{
    [SettingsGroup("CLIENT_STRING", "CLIENT_STRING_DESC", "ICON_HUNTERPIE")]
    public class ClientConfig : ISettings
    {
        [SettingField("LANGUAGE_STRING", "LANGUAGE_STRING_DESC")]
        public LanguagesFileSelector Language { get; set; } = new LanguagesFileSelector("en-us.xml");

        [SettingField("THEME_STRING", "THEME_STRING_DESC")] 
        public ThemeFileSelector Theme { get; set; } = new ThemeFileSelector("default");

        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray { get; set; }

    }
}
