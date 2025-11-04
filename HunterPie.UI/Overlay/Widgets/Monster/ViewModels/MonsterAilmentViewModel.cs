using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class MonsterAilmentViewModel : AutoVisibilityViewModel
{

    private bool _isEnabled = true;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private string _name;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private double _timer;
    public double Timer { get => _timer; set => SetValueAndRefresh(ref _timer, value); }

    private double _maxTimer;
    public double MaxTimer { get => _maxTimer; set => SetValue(ref _maxTimer, value); }

    private double _buildup;
    public double Buildup { get => _buildup; set => SetValueAndRefresh(ref _buildup, value); }

    private double _maxBuildup;
    public double MaxBuildup { get => _maxBuildup; set => SetValue(ref _maxBuildup, value); }

    private bool _isTimerActive;
    public bool IsTimerActive { get => _isTimerActive; set => SetValueAndRefresh(ref _isTimerActive, value); }

    private int _count;
    public int Count { get => _count; set => SetValue(ref _count, value); }

    public MonsterAilmentViewModel(MonsterWidgetConfig config) : base(config.AutoHideAilmentsDelay) { }
}