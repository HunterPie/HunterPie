using HunterPie.Core.Settings;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Enums;

namespace HunterPie.UI.Overlay.ViewModels;

public class WidgetViewModel : ViewModel
{
    public IWidgetSettings Settings { get; }

    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }

    private WidgetType _type;
    public WidgetType Type { get => _type; set => SetValue(ref _type, value); }

    public WidgetViewModel(IWidgetSettings settings)
    {
        Settings = settings;
    }
}