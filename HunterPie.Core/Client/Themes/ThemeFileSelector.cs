using HunterPie.Core.Settings;
using System;
using System.IO;

namespace HunterPie.Core.Client.Themes
{
    public class ThemeFileSelector : IFileSelector
    {
        public object Current { get; set; }

        public ThemeFileSelector(string @default)
        {
            Current = @default;
        }

        public object[] List()
        {
            if (!Directory.Exists(ClientInfo.ThemesPath))
                return Array.Empty<string>();

            return Directory.GetFiles(ClientInfo.ThemesPath, ".xaml");
        }
    }
}
