using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeHomeTabViewModel : ViewModel
{
    private string _icon = string.Empty;
    public required string Icon { get => _icon; set => SetValue(ref _icon, value); }

    private string _title = string.Empty;
    public string Title { get => _title; set => SetValue(ref _title, value); }
}