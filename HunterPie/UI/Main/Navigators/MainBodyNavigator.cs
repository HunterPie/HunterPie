using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Domain.Sidebar;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.Navigators.Events;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Main.Navigators;

internal class MainBodyNavigator : IBodyNavigator, IBodyNavigationDispatcher, IEventDispatcher
{
    private readonly ISideBarCollection _sideBar;
    private readonly Stack<ViewModel> _stack = new();

    public event EventHandler<NavigationRequestEventArgs>? NavigationRequest;

    public MainBodyNavigator(ISideBarCollection sideBar)
    {
        _sideBar = sideBar;
    }

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        _stack.Push(viewModel);

        foreach (ISideBarViewModel sideBarViewModel in _sideBar.Elements)
            sideBarViewModel.IsSelected = sideBarViewModel.Type == viewModel.GetType();

        this.Dispatch(
            toDispatch: NavigationRequest,
            data: new NavigationRequestEventArgs
            {
                ViewModel = viewModel
            });

    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        foreach (ISideBarViewModel sideBarViewModel in _sideBar.Elements)
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

    public void ReturnWhen<TViewModel>() where TViewModel : ViewModel
    {
        if (_stack.TryPeek(out ViewModel? vm) && vm is TViewModel)
            Return();
    }
}