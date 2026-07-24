using System.Collections.ObjectModel;

namespace HunterPie.Features.Extensions.ViewModels;

internal sealed class ExploreThemeHomeTabViewModel : ThemeHomeTabViewModel
{
    public ObservableCollection<ThemeCardViewModel> Themes { get; } = new();
}