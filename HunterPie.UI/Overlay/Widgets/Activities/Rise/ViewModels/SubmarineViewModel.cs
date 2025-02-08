using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise.ViewModels;

public class SubmarineViewModel : Bindable
{
    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    private int _maxCount;
    public int MaxCount { get => _maxCount; set => SetValue(ref _maxCount, value); }

    private int _daysLeft;
    public int DaysLeft { get => _daysLeft; set => SetValue(ref _daysLeft, value); }

    private bool _isActive;
    public bool IsActive { get => _isActive; set => SetValue(ref _isActive, value); }

    public ObservableCollection<SubmarineBoostViewModel> Boosts { get; } = new();
}