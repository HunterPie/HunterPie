using HunterPie.Core.Remote;
using HunterPie.DI;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel(
    IAccountUseCase accountUseCase,
    IAppNavigator appNavigator,
    IBodyNavigator bodyNavigator,
    SettingsSideBarViewModel settingsSideBarViewModel) : ViewModel
{
    private readonly IAccountUseCase _accountUseCase = accountUseCase;
    private readonly IAppNavigator _appNavigator = appNavigator;
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;
    private readonly SettingsSideBarViewModel _settingsSideBarViewModel = settingsSideBarViewModel;

    public bool IsLoading { get; set => SetValue(ref field, value); }
    public string AvatarUrl { get; set => SetValue(ref field, value); } = string.Empty;
    public string Username { get; set => SetValue(ref field, value); } = string.Empty;
    public bool IsLoggedIn { get; set => SetValue(ref field, value); }

    public bool IsOpen { get; set => SetValue(ref field, value); }

    public void OpenSignInScreen()
    {
        AccountSignFlowViewModel vm = DependencyContainer.Get<AccountSignFlowViewModel>();

        _appNavigator.Navigate(vm);
    }

    public async Task OpenAccountSettingsAsync()
    {
        await _settingsSideBarViewModel.ExecuteAsync();
    }

    public async void OpenAccountDetails()
    {
        AccountPreferencesViewModel viewModel = DependencyContainer.Get<AccountPreferencesViewModel>()
            .Apply(it => it.IsFetchingAccount = true);
        UserAccount? account = await _accountUseCase.GetAsync();

        if (account is not null)
            await viewModel.ApplyAsync(async it =>
            {
                it.Email = account.Email;
                it.Username = account.Username;
                it.AvatarUrl = await CDN.GetAsset(account.AvatarUrl);
                it.IsSupporter = account.IsSupporter;
                it.IsFetchingAccount = false;
            });


        _bodyNavigator.Navigate(viewModel);
    }

    public async void SignOut()
    {
        IsLoggedIn = false;
        await _accountUseCase.LogoutAsync();
    }
}