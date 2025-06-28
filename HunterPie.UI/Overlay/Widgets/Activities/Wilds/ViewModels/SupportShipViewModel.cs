using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;

public class SupportShipViewModel : ViewModel
{
    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }

    private int _days;
    public int Days { get => _days; set => SetValue(ref _days, value); }
}