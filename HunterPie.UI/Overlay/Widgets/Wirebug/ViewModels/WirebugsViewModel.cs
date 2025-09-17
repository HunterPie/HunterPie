using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Wirebug.ViewModels;

public class WirebugsViewModel : WidgetViewModel
{

    public ObservableCollection<WirebugViewModel> Elements { get; } = new();

    private bool _isAvailable;
    public bool IsAvailable { get => _isAvailable; set => SetValue(ref _isAvailable, value); }

    public WirebugsViewModel(IWidgetSettings settings) : base(settings, "Wirebugs Widget", WidgetType.ClickThrough)
    {
    }
}