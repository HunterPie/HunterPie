using HunterPie.Core.Client;
using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.Internal.Initializers
{
    internal class CustomThemeInitializer : IInitializer
    {
        public void Init()
        {
            string themePath = Path.Combine(ClientInfo.ThemesPath, ClientConfig.Config.Client.Theme);

            var xamlFilesToLoad = Directory.EnumerateFiles(themePath, "*.xaml");

            foreach (var file in xamlFilesToLoad)
                TryLoadingResource(file);

            Log.Info("Loaded theme {0}", ClientConfig.Config.Client.Theme.Current);
        }

        private void TryLoadingResource(string file)
        {
            try
            {
                using FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read);

                ResourceDictionary resource = (ResourceDictionary)XamlReader.Load(stream);
                Application.Current.Resources.MergedDictionaries.Add(resource);
            } catch(Exception err)
            {
                Log.Error("Failed to load custom file {0}\n{1}", file, err.ToString());
            }
            
        }
    }
}
