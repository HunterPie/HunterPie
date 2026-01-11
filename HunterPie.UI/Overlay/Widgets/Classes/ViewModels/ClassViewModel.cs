using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

#nullable enable
public class ClassViewModel(IWidgetSettings settings) : WidgetViewModel(settings, "Class Widget", WidgetType.ClickThrough)
{
    public IClassViewModel? Current { get; set => SetValue(ref field, value); }
    public bool InHuntingZone { get; set => SetValue(ref field, value); }
}