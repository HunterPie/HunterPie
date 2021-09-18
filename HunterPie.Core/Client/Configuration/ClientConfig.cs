using HunterPie.Core.Domain.Generics;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

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
        public Observable<bool> MinimizeToSystemTray { get; set; } = true;
    }
}
