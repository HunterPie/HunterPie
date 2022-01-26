using System;
using System.Windows;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class PatchNotesSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_CHANGELOG") as ImageSource;

        public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']").Attributes["String"].Value;

        public bool IsActivable => true;

        public bool IsEnabled => false;

        public void ExecuteOnClick()
        {

        }
    }
}
