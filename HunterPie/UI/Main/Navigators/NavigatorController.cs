using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;

namespace HunterPie.UI.Main.Navigators;

internal class NavigatorController
{
    private readonly IAppNavigationDispatcher _mainNavigationDispatcher;
    private readonly IAppNavigator _appNavigator;
    private readonly IBodyNavigationDispatcher _bodyNavigationDispatcher;
    private readonly IBodyNavigator _bodyNavigator;
    private readonly IAccountUseCase _accountUseCase;
    private readonly MainViewModel _mainViewModel;
    private readonly MainBodyViewModel _mainBodyViewModel;

    public NavigatorController(
        IAppNavigationDispatcher mainNavigationDispatcher,
        IAppNavigator appNavigator,
        IBodyNavigationDispatcher bodyNavigationDispatcher,
        IBodyNavigator bodyNavigator,
        IAccountUseCase accountUseCase,
        MainViewModel mainViewModel,
        MainBodyViewModel mainBodyViewModel)
    {
        _mainNavigationDispatcher = mainNavigationDispatcher;
        _bodyNavigationDispatcher = bodyNavigationDispatcher;
        _bodyNavigator = bodyNavigator;
        _accountUseCase = accountUseCase;
        _appNavigator = appNavigator;
        _mainViewModel = mainViewModel;
        _mainBodyViewModel = mainBodyViewModel;

        Subscribe();
    }

    private void Subscribe()
    {
        _mainNavigationDispatcher.NavigationRequest +=
            (_, args) => _mainViewModel.ContentViewModel = args.ViewModel;
        _bodyNavigationDispatcher.NavigationRequest +=
            (_, args) => _mainBodyViewModel.NavigationViewModel = args.ViewModel;
        _accountUseCase.SessionStart += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignIn += (_, e) => SetupViewModel(e.Account);
        _accountUseCase.SignOut += (_, _) => SetupViewModel(null);
    }

    private void SetupViewModel(UserAccount? account)
    {
        _mainBodyViewModel.InitializeSupporterPrompt(account?.IsSupporter ?? false);
    }
}