using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class IngredientsCenterViewModel : ViewModel
{
    private bool _isFull;
    public bool IsFull { get => _isFull; set => SetValue(ref _isFull, value); }

    private int _rations;
    public int Rations { get => _rations; set => SetValue(ref _rations, value); }

    private int _timer;
    public int Timer { get => _timer; set => SetValue(ref _timer, value); }
}