using HunterPie.Core.Logger;
using HunterPie.GUI.Parts.Console;
using HunterPie.GUI.Parts.Host;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels
{
    internal class ConsoleSideBarElementViewModel : ISideBarElement
    {
        public ImageSource Icon => Application.Current.FindResource("ICON_CONSOLE") as ImageSource;

        public string Text => "Console";

        public bool IsActivable => true;

        public void ExecuteOnClick()
        {
            var console = new ConsoleView();
            MainHost.SetMain(console);
        }
    }
}
