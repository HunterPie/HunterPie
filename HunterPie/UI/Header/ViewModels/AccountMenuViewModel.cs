using HunterPie.Core.Remote;
using HunterPie.Features.Account.Model;
using HunterPie.Features.Account.UseCase;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel : ViewModel
{
    private readonly IAccountUseCase _accountUseCase;

    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => SetValue(ref _isLoading, value); }

    private string _avatarUrl = string.Empty;
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }

    private string _username = string.Empty;
    public string Username { get => _username; set => SetValue(ref _username, value); }

    private bool _isLoggedIn;
    public bool IsLoggedIn { get => _isLoggedIn; set => SetValue(ref _isLoggedIn, value); }

    private bool _isOpen;

    public AccountMenuViewModel(
        IAccountUseCase accountUseCase
        )
    {
        _accountUseCase = accountUseCase;
    }

    public bool IsOpen { get => _isOpen; set => SetValue(ref _isOpen, value); }

    public void OpenSignInScreen()
    {
        Navigator.App.Navigate<AccountSignFlowViewModel>();
    }

    public void OpenAccountSettings()
    {
        Navigator.Body.Navigate<SettingsViewModel>();
    }

    public async void OpenAccountDetails()
    {
        UserAccount? account = await _accountUseCase.GetAsync();

        AccountPreferencesViewModel viewModel = account switch
        {
            { } => new AccountPreferencesViewModel
            {
                Email = account.Email,
                Username = account.Username,
                AvatarUrl = await CDN.GetAsset(account.AvatarUrl),
                IsSupporter = account.IsSupporter,
                IsFetchingAccount = false
            },

            _ => new AccountPreferencesViewModel { IsFetchingAccount = true }
        };

        Navigator.Body.Navigate(viewModel);
    }

    public async void SignOut()
    {
        IsLoggedIn = false;
        await _accountUseCase.LogoutAsync();
    }
}