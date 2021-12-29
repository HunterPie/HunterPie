using HunterPie.Core.Client;
using HunterPie.GUI.Parts.Host;
using HunterPie.Internal.Intializers;
using HunterPie.UI.Controls.Flags;
using HunterPie.UI.Controls.Settings;
using HunterPie.UI.Settings;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class SettingsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_SETTINGS") as ImageSource;

        public string Text => "Settings";
         
        public bool IsActivable => true;

        public bool IsEnabled => true;

        public void ExecuteOnClick()
        {
            var settingTabs = VisualConverterManager.Build(ClientConfig.Config);

            SettingHost host = new SettingHost();
            
            host.AddTab(settingTabs);

            // Also add feature flags
            host.AddTab(new FeatureFlagsView(FeatureFlagsInitializer.Features.Flags));
            
            MainHost.SetMain(host);
        }
    }
}
