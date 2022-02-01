using Microsoft.Win32;

namespace HunterPie.Core.Client
{
    public class RegistryConfig
    {

        internal static RegistryKey key;

        public static void Initialize()
        {
            key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\HunterPie");
        }

        public static void Set(string name, object value) => key.SetValue(name, value);
        public static bool Exists(string name) => key.GetValue(name) is not null;
        public static object Get(string name) => key.GetValue(name);

    }
}
