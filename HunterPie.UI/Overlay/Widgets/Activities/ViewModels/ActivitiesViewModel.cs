using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Common;

namespace HunterPie.UI.Overlay.Widgets.Activities.ViewModels;

#nullable enable
public class ActivitiesViewModel : WidgetViewModel
{
    private bool _inVisibleStage;
    public bool InVisibleStage { get => _inVisibleStage; set => SetValue(ref _inVisibleStage, value); }

    private IActivitiesViewModel? _activities;
    public IActivitiesViewModel? Activities { get => _activities; set => SetValue(ref _activities, value); }

    public ActivitiesViewModel(IWidgetSettings settings) : base(settings, "Activities Widget", WidgetType.ClickThrough)
    {

    }
}