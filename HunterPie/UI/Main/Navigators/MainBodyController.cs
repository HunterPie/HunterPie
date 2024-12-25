using HunterPie.Core.Extensions;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System.Collections.Generic;

namespace HunterPie.UI.Main.Navigators;

internal class MainBodyController : INavigator
{
    private readonly MainBodyViewModel _viewModel;
    private readonly IAccountUseCase _accountUseCase;
    private readonly Stack<ViewModel> _stack = new();

    public MainBodyController(
        MainBodyViewModel viewModel,
        IAccountUseCase accountUseCase)
    {
        _viewModel = viewModel;
        _accountUseCase = accountUseCase;

        Subscribe();
        SetupViewModel(null);
    }

    private void Subscribe()
    {
        _accountUseCase.SessionStart += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignIn += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignOut += (_, e) => SetupViewModel(null);
    }

    private void SetupViewModel(UserAccount? account)
    {
        _viewModel.InitializeSupporterPrompt(account?.IsSupporter ?? false);
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

    public void ReturnWhen<TViewModel>() where TViewModel : ViewModel
    {
        if (_stack.Peek() is TViewModel)
        {
            Return();
        }
    }
}