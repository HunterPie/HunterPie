using HunterPie.Core.Settings;
using System;
using System.IO;

namespace HunterPie.Core.Client.Language
{
    public class LanguagesFileSelector : IFileSelector
    {
        public object Current { get; set; }

        public LanguagesFileSelector(object @default)
        {
            Current = @default;
        }

        public object[] List()
        {
            if (!Directory.Exists(ClientInfo.LanguagesPath))
                return Array.Empty<string>();

            return Directory.GetFiles(ClientInfo.LanguagesPath, "*.xml");
        }
    }
}
