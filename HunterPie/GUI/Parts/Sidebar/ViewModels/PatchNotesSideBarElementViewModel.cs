using System;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class PatchNotesSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_CHANGELOG") as ImageSource;

        public string Text => "Patch Notes";

        public bool IsActivable => true;

        public void ExecuteOnClick()
        {

        }
    }
}
