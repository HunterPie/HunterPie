using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class ThemeHomeViewModel : ViewModel
{
    public ObservableCollection<ThemeHomeTabViewModel> Tabs { get; } = new();
}