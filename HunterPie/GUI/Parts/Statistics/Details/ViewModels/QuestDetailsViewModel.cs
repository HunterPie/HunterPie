using HunterPie.GUI.Parts.Host;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
public class QuestDetailsViewModel : ViewModel
{
    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();

    public void NavigateToPreviousPage() => MainHost.Return();
}
