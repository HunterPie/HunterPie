using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class MonsterAilmentViewModel(MonsterWidgetConfig config) : AutoVisibilityViewModel(config.AutoHideAilmentsDelay)
{
    public bool IsEnabled { get; set => SetValue(ref field, value); } = true;
    public string Name { get; set => SetValue(ref field, value); }
    public double Timer { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxTimer { get; set => SetValue(ref field, value); }
    public double Buildup { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxBuildup { get; set => SetValue(ref field, value); }
    public bool IsTimerActive { get; set => SetValueAndRefresh(ref field, value); }
    public int Count { get; set => SetValue(ref field, value); }
}