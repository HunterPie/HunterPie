using HunterPie.Core.Extensions;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Main;

internal class MainController : INavigator
{
    private readonly Dictionary<Type, ViewModel> _viewModels = new();
    private readonly Stack<ViewModel> _stack = new();
    public readonly MainViewModel ViewModel;
    public readonly MainView View;

    public MainController(MainView view, MainViewModel viewModel)
    {
        View = view;
        ViewModel = viewModel;
    }

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        Type viewModelType = viewModel.GetType();

        if (_viewModels.ContainsKey(viewModelType))
            _viewModels.Remove(viewModelType);

        _viewModels.Add(viewModelType, viewModel);
        ViewModel.ContentViewModel = viewModel;
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        if (!_viewModels.ContainsKey(typeof(TViewModel)))
            return;

        Navigate(_viewModels[typeof(TViewModel)]);
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
        if (_stack.Peek() is TViewModel)
        {
            Return();
        }
    }
}