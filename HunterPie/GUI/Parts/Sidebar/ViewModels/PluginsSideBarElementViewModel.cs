using HunterPie.UI.Assets.Application;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class PluginsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Resources.Icon("ICON_PLUGIN");

        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='PLUGINS_STRING']").Attributes["String"].Value;

        public bool IsActivable => true;

        public bool IsEnabled => false;

        public bool ShouldNotify => false;

        public void ExecuteOnClick()
        {
            
        }
    }
}
