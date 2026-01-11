using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Statistics.Details.ViewModels;

internal class QuestDetailsViewModel : ViewModel
{
    public int SelectedIndex { get; set => SetValue(ref field, value); }

    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();

    [System.Obsolete]
    public void NavigateToPreviousPage() => Navigator.Body.Return();
}