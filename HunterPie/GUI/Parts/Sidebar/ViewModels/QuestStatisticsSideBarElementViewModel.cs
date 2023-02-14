using HunterPie.GUI.Parts.Host;
using HunterPie.GUI.Parts.Statistics.Views;
using HunterPie.UI.Architecture;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class QuestStatisticsSideBarElementViewModel : ViewModel, ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_PATREON");
    public string Text => "Test";
    public bool IsActivable => true;
    public bool IsEnabled => true;
    public bool ShouldNotify { get; }

    public void ExecuteOnClick()
    {
        MonsterSummaryView view = new();

        MainHost.SetMain(view);
    }
}
