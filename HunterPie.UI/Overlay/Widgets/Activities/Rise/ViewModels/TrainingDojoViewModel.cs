using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class TrainingDojoViewModel : ViewModel
{
    public int Boosts { get; set => SetValue(ref field, value); }
    public int MaxBoosts { get; set => SetValue(ref field, value); }
    public int Rounds { get; set => SetValue(ref field, value); }
    public int MaxRounds { get; set => SetValue(ref field, value); }
}