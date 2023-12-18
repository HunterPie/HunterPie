using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;

namespace HunterPie.UI.Main;

internal class MainBodyNavigator : INavigator
{
    private readonly MainBodyViewModel _viewModel;

    public MainBodyNavigator(MainBodyViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        _viewModel.NavigationViewModel = viewModel;

        foreach (ISideBarViewModel sideBarViewModel in _viewModel.SideBarViewModel.Elements)
            sideBarViewModel.IsSelected = sideBarViewModel.Type == typeof(TViewModel);
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        foreach (ISideBarViewModel sideBarViewModel in _viewModel.SideBarViewModel.Elements)
        {
            sideBarViewModel.IsSelected = sideBarViewModel.Type == typeof(TViewModel);
            if (sideBarViewModel.IsSelected)
                sideBarViewModel.Execute();
        }
    }
}