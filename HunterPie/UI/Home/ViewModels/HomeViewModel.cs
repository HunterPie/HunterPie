using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeViewModel(
    ObservableCollection<SupportedGameViewModel> supportedGames,
    ObservableCollection<HomeCallToActionViewModel> quickActions
    ) : ViewModel
{
    public ObservableCollection<SupportedGameViewModel> SupportedGames { get; } = supportedGames;

    public ObservableCollection<HomeCallToActionViewModel> QuickActions { get; } = quickActions;

    public ObservableCollection<HomeNewsItemViewModel> News { get; } = new()
    {
        new HomeNewsItemViewModel
        {
            Banner = "https://cdn.hunterpie.com/Static/update-2.11.0-banner.png",
            Description = "Click here to read the patch notes",
            Title = "Update v2.11.0"
        },
        new HomeNewsItemViewModel
        {
            Banner = "https://cdn.hunterpie.com/resources/patreon-banner.png",
            Description = "Read more...",
            Title = "Patreon"
        }
    };
}