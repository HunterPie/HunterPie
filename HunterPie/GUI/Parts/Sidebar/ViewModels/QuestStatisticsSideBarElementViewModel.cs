using HunterPie.GUI.Parts.Host;
using HunterPie.GUI.Parts.Statistics.Views;
using HunterPie.UI.Architecture;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class QuestStatisticsSideBarElementViewModel : ViewModel, ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_BUILD");
    public string Text => "Hunts";
    public bool IsActivable => true;
    public bool IsEnabled => true;
    public bool ShouldNotify { get; }

    public void ExecuteOnClick()
    {
        QuestStatisticsSummariesView view = new();

        MainHost.SetMain(view);
    }
}
