using HunterPie.Core.API.Entities;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Model;
using HunterPie.UI.Architecture;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Account.ViewModels;

#nullable enable
public class AccountLoginFlowViewModel : ViewModel
{
    private string _username = "";
    private string _password = "";
    private bool _isLoggingIn;

    public string Username { get => _username; set => SetValue(ref _username, value); }
    public string Password { get => _password; set => SetValue(ref _password, value); }
    public bool IsLoggingIn { get => _isLoggingIn; set => SetValue(ref _isLoggingIn, value); }

    public async Task<bool> SignIn()
    {
        IsLoggingIn = true;

        var request = new LoginRequest
        {
            Username = Username,
            Password = Password
        };

        UserAccount? result = await AccountLoginManager.Login(request);

        IsLoggingIn = false;

        return result is not null;
    }
}
#nullable restore
