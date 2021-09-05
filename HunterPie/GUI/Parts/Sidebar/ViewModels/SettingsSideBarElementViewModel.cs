using HunterPie.Core.Logger;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class SettingsSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_SETTINGS") as ImageSource;

        public string Text => "Settings";

        public bool IsActivable => true;

        public void ExecuteOnClick()
        {
            Log.Debug("Settings button click!");
        }
    }
}
