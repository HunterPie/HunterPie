using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class TrainingDojoViewModel : ViewModel
{
    private int _boosts;
    public int Boosts { get => _boosts; set => SetValue(ref _boosts, value); }

    private int _maxBoosts;
    public int MaxBoosts { get => _maxBoosts; set => SetValue(ref _maxBoosts, value); }

    private int _rounds;
    public int Rounds { get => _rounds; set => SetValue(ref _rounds, value); }

    private int _maxRounds;
    public int MaxRounds { get => _maxRounds; set => SetValue(ref _maxRounds, value); }
}