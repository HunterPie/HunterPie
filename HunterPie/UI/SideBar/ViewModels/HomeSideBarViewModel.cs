using HunterPie.UI.Architecture;
using HunterPie.UI.Home.Services;
using HunterPie.UI.Home.ViewModels;
using HunterPie.UI.Navigation;
using System;
using System.Collections.ObjectModel;

namespace HunterPie.UI.SideBar.ViewModels;

internal class HomeSideBarViewModel : ViewModel, ISideBarViewModel
{
    private readonly HomeService _homeService;
    private readonly HomeCallToActionsService _callToActionsService;

    public Type Type => typeof(HomeViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='HOME_STRING']";

    public string Icon => "ICON_HOME";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public HomeSideBarViewModel(
        HomeService homeService,
        HomeCallToActionsService callToActionsService)
    {
        _homeService = homeService;
        _callToActionsService = callToActionsService;

        _homeService.Subscribe();
    }

    public void Execute()
    {
        ObservableCollection<SupportedGameViewModel> supportedGames = _homeService.GetSupportedGameViewModels();
        ObservableCollection<HomeCallToActionViewModel> quickActions = _callToActionsService.GetAll();
        var homeViewModel = new HomeViewModel(supportedGames, quickActions);

        Navigator.Body.Navigate(homeViewModel);
    }
}