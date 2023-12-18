using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Main.Views;
using HunterPie.UI.Navigation;
using System;
using System.Collections.Specialized;

namespace HunterPie.UI.Main;

internal class MainController : INavigator
{
    private readonly OrderedDictionary _viewModels = new OrderedDictionary();
    public readonly MainViewModel ViewModel;
    public readonly MainView View;

    public MainController(MainView view, MainViewModel viewModel)
    {
        View = view;
        ViewModel = viewModel;
    }

    public void Navigate<TViewModel>(TViewModel viewModel) where TViewModel : ViewModel
    {
        Type viewModelType = typeof(TViewModel);

        if (_viewModels.Contains(viewModelType))
            _viewModels.Remove(viewModelType);

        _viewModels.Add(viewModelType, viewModel);
        ViewModel.ContentViewModel = viewModel;
    }

    public void Navigate<TViewModel>() where TViewModel : ViewModel
    {
        if (!_viewModels.Contains(typeof(TViewModel)))
            return;

        ViewModel.ContentViewModel = _viewModels[typeof(TViewModel)] as ViewModel;
    }
}