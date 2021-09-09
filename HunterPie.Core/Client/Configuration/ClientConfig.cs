using HunterPie.Core.Domain.Generics;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration
{
    [SettingsGroup("CLIENT_STRING", "CLIENT_STRING_DESC", "ICON_HUNTERPIE")]
    public class ClientConfig : ISettings
    {
        [SettingField("LANGUAGE_STRING", "LANGUAGE_STRING_DESC")]
        public GenericFileSelector Language { get; set; } = new GenericFileSelector("en-us.xml", "*.xml", ClientInfo.LanguagesPath);

        [SettingField("THEME_STRING", "THEME_STRING_DESC")] 
        public GenericFileSelector Theme { get; set; } = new GenericFileSelector("default", "*.xaml", ClientInfo.ThemesPath);

        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray { get; set; }

        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray1 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray2 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray3 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray4 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray5 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray6 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray7 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray8 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray9 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray10 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray11 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray12 { get; set; }
        [SettingField("MINIMIZE_TO_SYSTEM_TRAY_STRING", "MINIMIZE_TO_SYSTEM_TRAY_STRING_DESC")]
        public bool MinimizeToSystemTray13 { get; set; }

    }
}
