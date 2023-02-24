using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
internal class QuestDetailsViewModel : ViewModel
{
    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();
}
