using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeHomeTabViewModel : ViewModel
{
    public required string Icon { get; set => SetValue(ref field, value); } = string.Empty;
    public string Title { get; set => SetValue(ref field, value); } = string.Empty;
}