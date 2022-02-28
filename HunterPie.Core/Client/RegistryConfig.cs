using Microsoft.Win32;
using System;

namespace HunterPie.Core.Client
{
    public class RegistryConfig
    {

        internal static RegistryKey key;

        public static void Initialize()
        {
            key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\HunterPie");
        }

        public static void Set<T>(string name, T value) => key.SetValue(name, value);
        public static bool Exists(string name) => key.GetValue(name) is not null;
        public static object Get(string name) => key.GetValue(name);
        public static T Get<T>(string name) => (T)Convert.ChangeType(key.GetValue(name), typeof(T));

    }
}
