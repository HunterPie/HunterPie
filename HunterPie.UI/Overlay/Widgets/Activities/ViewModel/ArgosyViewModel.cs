using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModel;

public class ArgosyViewModel : Bindable
{
    private int _daysLeft;
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }
}