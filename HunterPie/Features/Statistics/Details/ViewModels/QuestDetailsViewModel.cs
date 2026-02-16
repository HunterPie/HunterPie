using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Statistics.Details.ViewModels;

internal class QuestDetailsViewModel(IBodyNavigator bodyNavigator) : ViewModel
{
    public int SelectedIndex { get; set => SetValue(ref field, value); }

    public ObservableCollection<MonsterDetailsViewModel> Monsters { get; } = new();

    public void NavigateToPreviousPage() => bodyNavigator.Return();
}