using System;
using System.IO;
using System.Reflection;

namespace HunterPie.Core.Client
{
    public static class ClientInfo
    {
        public static string ClientPath => AppDomain.CurrentDomain.BaseDirectory;
        public static string PluginsPath => Path.Combine(ClientPath, "Modules");
        public static string LanguagesPath => Path.Combine(ClientPath, "Languages");
        public static string AddressPath => Path.Combine(ClientPath, "Address");
        public static string ThemesPath => Path.Combine(ClientPath, "Themes");
        public static Version Version
        {
            get
            {
                Assembly self = Assembly.GetEntryAssembly();
                AssemblyName name = self.GetName();
                return name.Version;
            }
        }
        public const string ConfigName = "config.json";
        public const string ConfigBackupName = ConfigName + ".bak";

        public static bool IsVersionGreaterOrEq(Version other)
        {
            Assembly self = Assembly.GetEntryAssembly();
            AssemblyName name = self.GetName();
            Version ver = name.Version;

            return ver >= other;
        }

        public static string GetPathFor(string relative)
        {
            return Path.Combine(ClientPath, relative);
        }
    }
}
