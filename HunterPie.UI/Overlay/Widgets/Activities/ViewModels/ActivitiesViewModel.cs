using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModels;

#nullable enable
public class ActivitiesViewModel(IWidgetSettings settings) : WidgetViewModel(settings, "Activities Widget", WidgetType.ClickThrough)
{
    public bool InVisibleStage { get; set => SetValue(ref field, value); }
    public IActivitiesViewModel? Activities { get; set => SetValue(ref field, value); }
}