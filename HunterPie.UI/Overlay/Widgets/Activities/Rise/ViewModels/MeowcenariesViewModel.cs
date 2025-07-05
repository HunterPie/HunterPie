using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class MeowcenariesViewModel : ViewModel
{
    private int _step;
    public int Step { get => _step; set => SetValue(ref _step, value); }

    private int _maxSteps;
    public int MaxSteps { get => _maxSteps; set => SetValue(ref _maxSteps, value); }

    public ObservableCollection<bool> Outcome { get; } = new();

    private int _buddyCount;
    public int BuddyCount { get => _buddyCount; set => SetValue(ref _buddyCount, value); }

    private int _maxBuddyCount;
    public int MaxBuddyCount { get => _maxBuddyCount; set => SetValue(ref _maxBuddyCount, value); }

    private bool _isDeployed;
    public bool IsDeployed { get => _isDeployed; set => SetValue(ref _isDeployed, value); }

    public void SetOutcome(int current, int max)
    {
        Outcome.Clear();

        for (int i = 0; i < max; i++)
            Outcome.Add(i > current);
    }
}