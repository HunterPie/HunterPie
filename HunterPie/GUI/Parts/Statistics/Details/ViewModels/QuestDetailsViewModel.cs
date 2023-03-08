using HunterPie.GUI.Parts.Host;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;
public class QuestDetailsViewModel : ViewModel
{
    private int _selectedIndex;
    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }

    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();

    public void NavigateToPreviousPage() => MainHost.Return();
}
