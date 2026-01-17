using HunterPie.Features.Account.UseCase;
using HunterPie.Integrations.Poogie.Account.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.UI.Architecture;
using HunterPie.UI.Main.ViewModels;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Account.ViewModels;

internal class AccountLoginFlowViewModel(
    AccountPasswordResetFlowViewModel passwordResetFlowViewModel,
    IAccountUseCase accountUseCase,
    AccountVerificationResendFlowViewModel accountVerificationResendFlowViewModel) : ViewModel
{
    public string Email
    {
        get;
        set
        {
            CanLogIn = Password.Length > 0 && value.Length > 0;
            SetValue(ref field, value);
        }
    } = "";
    public string Password
    {
        get;
        set
        {
            CanLogIn = Email.Length > 0 && value.Length > 0;
            SetValue(ref field, value);
        }
    } = "";
    public bool IsLoggingIn { get; set => SetValue(ref field, value); }
    public bool CanLogIn { get; set => SetValue(ref field, value); }

    private readonly IAccountUseCase _accountUseCase = accountUseCase;
    public AccountPasswordResetFlowViewModel PasswordResetFlowViewModel { get; } = passwordResetFlowViewModel;
    public AccountVerificationResendFlowViewModel AccountVerificationResendFlowViewModel { get; } = accountVerificationResendFlowViewModel;

    public async Task<bool> SignIn()
    {
        if (!CanLogIn)
            return false;

        IsLoggingIn = true;
        CanLogIn = false;

        var request = new LoginRequest
        (
            Email: Email,
            Password: Password
        );
        PoogieResult<LoginResponse>? result = await _accountUseCase.LoginAsync(request);

        IsLoggingIn = false;
        CanLogIn = true;

        if (result is null || result.Error is { })
            return false;

        Navigator.App.Navigate<MainBodyViewModel>();

        return true;
    }

    public void NavigateToPasswordResetFlow() => PasswordResetFlowViewModel.IsFlowActive = true;

    public void NavigateToAccountVerificationFlow() => AccountVerificationResendFlowViewModel.IsFlowActive = true;
}