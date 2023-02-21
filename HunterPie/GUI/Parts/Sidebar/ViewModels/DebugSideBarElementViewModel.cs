
using HunterPie.GUI.Parts.Debug.Views;
using HunterPie.GUI.Parts.Host;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class DebugSideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_BUG");
    public string Text => "Debug";
    public bool IsActivable => true;
    public bool IsEnabled => true;
    public bool ShouldNotify => false;

    public void ExecuteOnClick()
    {
        var view = new EventTrackerView();
        MainHost.SetMain(view);
    }
}