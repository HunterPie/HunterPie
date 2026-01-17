using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;

public class WirebugsViewModel(IWidgetSettings settings) : WidgetViewModel(settings, "Wirebugs Widget", WidgetType.ClickThrough)
{

    public ObservableCollection<WirebugViewModel> Elements { get; } = new();
    public bool IsAvailable { get; set => SetValue(ref field, value); }
}