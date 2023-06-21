using HunterPie.GUI.Parts.Console;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class ConsoleSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_CONSOLE");

    public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='CONSOLE_STRING']").Attributes["String"].Value;

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => false;

    public void ExecuteOnClick()
    {
        var console = new ConsoleView();
        Navigator.Navigate(console);
    }
}
