using System;
using System.IO;

namespace HunterPie.Core.Client
{
    public static class ClientInfo
    {
        public static string ClientPath => AppDomain.CurrentDomain.BaseDirectory;
        public static string PluginsPath => Path.Combine(ClientPath, "Modules");
        public static string LanguagesPath => Path.Combine(ClientPath, "Languages");
        public static string AddressPath => Path.Combine(ClientPath, "Address");
        public static string ThemesPath => Path.Combine(ClientPath, "Themes");
        public const string ConfigName = "config.json";
        public const string ConfigBackupName = ConfigName + ".bak";
    }
}
