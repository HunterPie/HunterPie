using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class MonsterPartViewModel(MonsterWidgetConfig config) : AutoVisibilityViewModel(config.AutoHidePartsDelay)
{
    public bool IsEnabled { get; set => SetValue(ref field, value); }
    public string Name { get; set => SetValue(ref field, value); }
    public double Health { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxHealth { get; set => SetValue(ref field, value); }
    public double Tenderize { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxTenderize { get; set => SetValue(ref field, value); }
    public double Flinch { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxFlinch { get; set => SetValue(ref field, value); }
    public double Sever { get; set => SetValueAndRefresh(ref field, value); }
    public double MaxSever { get; set => SetValue(ref field, value); }
    public double QurioHealth { get; set => SetValueAndRefresh(ref field, value); }
    public double QurioMaxHealth { get; set => SetValue(ref field, value); }
    public int Breaks { get; set => SetValueAndRefresh(ref field, value); }
    public int MaxBreaks { get; set => SetValue(ref field, value); }
    public bool IsPartBroken { get; set => SetValue(ref field, value); }
    public bool IsPartSevered { get; set => SetValue(ref field, value); }
    public bool IsKnownPart { get; set => SetValue(ref field, value); }
    public PartType Type { get; set => SetValue(ref field, value); }
}