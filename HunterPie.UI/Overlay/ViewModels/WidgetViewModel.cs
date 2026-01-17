using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;

namespace HunterPie.UI.Overlay.ViewModels;

public class WidgetViewModel : ViewModel
{
    public IWidgetSettings Settings { get; set => SetValue(ref field, value); }
    public string Title { get; set => SetValue(ref field, value); } = string.Empty;
    public WidgetType Type { get; set => SetValue(ref field, value); }

    public WidgetViewModel(
        IWidgetSettings settings,
        string title,
        WidgetType type)
    {
        Settings = settings;
        Title = title;
        Type = type;
    }
}