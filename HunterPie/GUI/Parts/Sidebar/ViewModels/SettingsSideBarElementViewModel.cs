using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.GUI.Parts.Host;
using HunterPie.Internal.Initializers;
using HunterPie.UI.Controls.Flags;
using HunterPie.UI.Controls.Settings;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class SettingsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_SETTINGS") as ImageSource;

        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']").Attributes["String"].Value;
         
        public bool IsActivable => true;

        public bool IsEnabled => true;

        public bool ShouldNotify => false;

        public SettingsSideBarElementViewModel()
        {
            ClientConfig.Config.Client.DefaultGameType.PropertyChanged += (_, __) => RefreshSettingsWindow(true);
        }

        public void ExecuteOnClick() => RefreshSettingsWindow();

        private void RefreshSettingsWindow(bool forceRefresh = false)
        {
            var settingTabs = VisualConverterManager.Build(ClientConfig.Config);

            var gameSpecificTabs = VisualConverterManager.Build(
                ClientConfig.Config.Client.DefaultGameType.Value switch
                {
                    GameType.Rise => ClientConfig.Config.Rise,
                    GameType.World => ClientConfig.Config.World,
                    _ => throw new System.NotImplementedException(),
                }
            );

            settingTabs = settingTabs.Concat(gameSpecificTabs)
                .ToArray();

            var _ = ClientConfig.Config.Client.Language;

            SettingHostViewModel vm = new(settingTabs);
            SettingHost host = new SettingHost()
            {
                DataContext = vm
            };

            // Also add feature flags if enabled
            if (ClientConfig.Config.Client.EnableFeatureFlags)
                vm.Elements.Add(new FeatureFlagsView(FeatureFlagsInitializer.Features.Flags));

            MainHost.SetMain(host, forceRefresh);
        }
    }
}
