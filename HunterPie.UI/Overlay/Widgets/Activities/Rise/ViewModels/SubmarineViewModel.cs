using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarineViewModel : ViewModel
{
    public int Count { get; set => SetValue(ref field, value); }
    public int MaxCount { get; set => SetValue(ref field, value); }
    public int DaysLeft { get; set => SetValue(ref field, value); }
    public bool IsActive { get; set => SetValue(ref field, value); }

    public ObservableCollection<SubmarineBoostViewModel> Boosts { get; } = new();

    public void SetMaxBoosts(int count)
    {
        if (Boosts.Count == count)
            return;

        for (int i = 0; i < count; i++)
            Boosts.Add(new SubmarineBoostViewModel());
    }

    public void SetBoosts(int normalCount, int extraCount)
    {
        for (int i = 0; i < Boosts.Count; i++)
        {
            SubmarineBoostViewModel viewModel = Boosts[i];
            viewModel.IsActive = i < normalCount + extraCount;
            viewModel.IsExtraBoost = viewModel.IsActive && i >= normalCount;
        }
    }
}