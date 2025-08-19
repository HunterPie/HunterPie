using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeHomeTabViewModel : ThemeHomeTabViewModel
{
    public ObservableCollection<InstalledThemeViewModel> Themes { get; } = new();
}