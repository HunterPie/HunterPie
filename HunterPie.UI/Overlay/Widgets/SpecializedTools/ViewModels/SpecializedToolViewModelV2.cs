using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

public class SpecializedToolViewModelV2(SpecializedToolWidgetConfig settings) : WidgetViewModel(settings, "Specialized Tool Widget", WidgetType.ClickThrough)
{
    public SpecializedToolType Id { get; set => SetValue(ref field, value); }
    public double Timer { get; set => SetValue(ref field, value); }
    public double MaxTimer { get; set => SetValue(ref field, value); }
    public double Cooldown { get; set => SetValue(ref field, value); }
    public double MaxCooldown { get; set => SetValue(ref field, value); }
    public bool IsRecharging { get; set => SetValue(ref field, value); }
    public bool IsVisible { get; set => SetValue(ref field, value); }

    public SpecializedToolWidgetConfig Config { get; } = settings;
}