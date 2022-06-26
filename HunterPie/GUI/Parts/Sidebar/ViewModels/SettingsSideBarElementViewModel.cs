using HunterPie.Core.Architecture;
using HunterPie.Core.Client;
using HunterPie.GUI.Parts.Host;
using HunterPie.Internal.Initializers;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Controls.Flags;
using HunterPie.UI.Controls.Settings;
using HunterPie.UI.Controls.Settings.ViewModel;
using HunterPie.UI.Settings;
using System;
using System.Linq;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class SettingsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Resources.Icon("ICON_SETTINGS");

        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='SETTINGS_STRING']").Attributes["String"].Value;
         
        public bool IsActivable => true;

        public bool IsEnabled => true;

        public bool ShouldNotify => false;

        public SettingsSideBarElementViewModel()
        {
            RefreshWindowOnChange(ClientConfig.Config.Client.LastConfiguredGame);
            RefreshWindowOnChange(ClientConfig.Config.Client.EnableFeatureFlags);
        }

        public void ExecuteOnClick() => RefreshSettingsWindow();

        private void RefreshSettingsWindow(bool forceRefresh = false)
        {
            var settingTabs = VisualConverterManager.Build(ClientConfig.Config);

            var gameSpecificTabs = VisualConverterManager.Build(
                ClientConfigHelper.GetGameConfigBy(ClientConfig.Config.Client.LastConfiguredGame.Value)
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

        private void RefreshWindowOnChange(Bindable observable) => observable.PropertyChanged += (_, __) => RefreshSettingsWindow(true);

    }
}
