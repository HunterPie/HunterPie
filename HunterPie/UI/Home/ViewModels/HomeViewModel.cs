using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Home.ViewModels;

internal class HomeViewModel : ViewModel
{
    public ObservableCollection<SupportedGameViewModel> SupportedGames { get; }

    public ObservableCollection<string> News { get; } = new()
    {
        "https://www.monsterhunter.com/rise/assets/images/mv-am.jpg",
        "https://www.monsterhunter.com/rise/assets/images/mv-am.jpg",
        "https://www.monsterhunter.com/rise/assets/images/mv-am.jpg"
    };

    public HomeViewModel(ObservableCollection<SupportedGameViewModel> supportedGames)
    {
        SupportedGames = supportedGames;
    }
}