using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class IngredientsCenterViewModel : ViewModel
{
    public bool IsFull { get; set => SetValue(ref field, value); }
    public int Rations { get; set => SetValue(ref field, value); }
    public int Timer { get; set => SetValue(ref field, value); }
}