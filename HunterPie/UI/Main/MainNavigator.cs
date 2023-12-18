using HunterPie.Core.Extensions;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System.Collections.Generic;

namespace HunterPie.UI.Main;

internal class MainBodyNavigator : INavigator
{
    private readonly MainBodyViewModel _viewModel;
    private readonly Stack<ViewModel> _stack = new();

    public MainBodyNavigator(MainBodyViewModel viewModel)
    {
        _viewModel = viewModel;
    }

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        _stack.Push(viewModel);
        _viewModel.NavigationViewModel = viewModel;

        foreach (ISideBarViewModel sideBarViewModel in _viewModel.SideBarViewModel.Elements)
            sideBarViewModel.IsSelected = sideBarViewModel.Type == viewModel.GetType();
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

    public void Return()
    {
        _stack.PopOrDefault();

        if (!_stack.TryPop(out ViewModel? viewModel))
            return;

        Navigate(viewModel);
    }
}