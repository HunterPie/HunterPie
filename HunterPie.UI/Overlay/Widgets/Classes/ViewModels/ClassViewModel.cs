using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Classes.ViewModels;

#nullable enable
public class ClassViewModel : WidgetViewModel
{
    private IClassViewModel? _current;
    public IClassViewModel? Current { get => _current; set => SetValue(ref _current, value); }

    private bool _inHuntingZone;
    public bool InHuntingZone { get => _inHuntingZone; set => SetValue(ref _inHuntingZone, value); }

    public ClassViewModel(IWidgetSettings settings) : base(settings, "Class Widget", WidgetType.ClickThrough)
    {
    }
}