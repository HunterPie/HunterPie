using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal sealed class ExploreThemeHomeTabViewModel : ThemeHomeTabViewModel
{
    public ObservableCollection<ThemeCardViewModel> Themes { get; } = new();
}