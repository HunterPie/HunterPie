using HunterPie.Core.Client.Localization;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Home.Services;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace HunterPie.UI.Home.Navigation;

internal class HomeNavigationHandler(
    IBodyNavigator navigator,
    HomeService service,
    HomeCallToActionsService ctaService,
    ILocalizationRepository localizationRepository
) : NavigationHandler.View<HomeViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='HOME_STRING']"),
    icon: Resources.Icon("ICON_HOME")
)
{
    public override Task InitializeAsync()
    {
        service.Subscribe();
        State = SideBarButtonState.Enabled;
        return Task.CompletedTask;
    }

    public override Task ExecuteAsync()
    {
        ObservableCollection<SupportedGameViewModel> supportedGames = service.GetSupportedGameViewModels();
        ObservableCollection<HomeCallToActionViewModel> quickActions = ctaService.GetAll();
        var homeViewModel = new HomeViewModel(
            supportedGames: supportedGames,
            quickActions: quickActions
        );

        navigator.Navigate(homeViewModel);

        return Task.CompletedTask;
    }
}