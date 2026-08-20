using HunterPie.Core.Assets;
using HunterPie.DI;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Account.ViewModels;
using HunterPie.Features.Settings.Navigation;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Extensions;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel(
    IAccountUseCase accountUseCase,
    IAppNavigator appNavigator,
    IBodyNavigator bodyNavigator,
    SettingsNavigationHandler settingsSideBarViewModel,
    IAssetResolver assetResolver
) : ViewModel
{
    public bool IsLoading { get; set => SetValue(ref field, value); }
    public string AvatarUrl { get; set => SetValue(ref field, value); } = string.Empty;
    public string Username { get; set => SetValue(ref field, value); } = string.Empty;
    public bool IsLoggedIn { get; set => SetValue(ref field, value); }

    public bool IsOpen { get; set => SetValue(ref field, value); }

    public void OpenSignInScreen()
    {
        AccountSignFlowViewModel vm = DependencyContainer.Get<AccountSignFlowViewModel>();

        appNavigator.Navigate(vm);
    }

    public async Task OpenAccountSettingsAsync()
    {
        await settingsSideBarViewModel.ExecuteAsync();
    }

    public async void OpenAccountDetails()
    {
        AccountPreferencesViewModel viewModel = DependencyContainer.Get<AccountPreferencesViewModel>()
            .Apply(it => it.IsFetchingAccount = true);
        UserAccount? account = await accountUseCase.GetAsync();

        if (account is not null)
            await viewModel.ApplyAsync(async it =>
            {
                var uri = new Uri(account.AvatarUrl);

                it.Email = account.Email;
                it.Username = account.Username;
                it.AvatarUrl = await assetResolver.Resolve(uri.AbsolutePath);
                it.IsSupporter = account.IsSupporter;
                it.IsFetchingAccount = false;
            });


        bodyNavigator.Navigate(viewModel);
    }

    public async void SignOut()
    {
        IsLoggedIn = false;
        await accountUseCase.LogoutAsync();
    }
}