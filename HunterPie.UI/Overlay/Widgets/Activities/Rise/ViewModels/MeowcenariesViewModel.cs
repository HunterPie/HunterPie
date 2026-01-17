using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MeowcenariesViewModel : ViewModel
{
    public int Step { get; set => SetValue(ref field, value); }
    public int MaxSteps { get; set => SetValue(ref field, value); }

    public ObservableCollection<bool> Outcome { get; } = new();
    public int BuddyCount { get; set => SetValue(ref field, value); }
    public int MaxBuddyCount { get; set => SetValue(ref field, value); }
    public bool IsDeployed { get; set => SetValue(ref field, value); }

    public void SetOutcome(int current, int max)
    {
        Outcome.Clear();

        for (int i = 0; i < max; i++)
            Outcome.Add(i > current);
    }
}