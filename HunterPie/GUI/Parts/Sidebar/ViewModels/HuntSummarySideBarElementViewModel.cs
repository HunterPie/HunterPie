using HunterPie.Features.Statistics.ViewModels;
using HunterPie.Features.Statistics.Views;
using HunterPie.GUI.Parts.Host;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;
internal class HuntSummarySideBarElementViewModel : ISideBarElement
{
    public ImageSource Icon => Resources.Icon("ICON_HUNTERPIE");

    public string Text => "Hunt Summary";

    public bool IsActivable => true;

    public bool IsEnabled => true;

    public bool ShouldNotify => true;

    public void ExecuteOnClick()
    {
        HuntSummaryView view = new()
        {
            DataContext = new MockHuntSummaryViewModel()
        };
        MainHost.SetMain(view);
    }
}
