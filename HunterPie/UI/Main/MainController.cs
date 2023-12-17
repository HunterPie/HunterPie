using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.UI.Main;

internal class MainController : INavigator
{
    public readonly MainViewModel ViewModel;
    public readonly MainView View;

    public MainController(MainView view, MainViewModel viewModel)
    {
        View = view;
        ViewModel = viewModel;
    }

    public void Navigate<TViewModel>(TViewModel viewModel)
        where TViewModel : ViewModel
    {
        ViewModel.NavigationViewModel = viewModel;

        foreach (ISideBarViewModel sideBarViewModel in ViewModel.SideBarViewModel.Elements)
            sideBarViewModel.IsSelected = sideBarViewModel.Type == typeof(TViewModel);
    }
}