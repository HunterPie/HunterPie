using HunterPie.Core.API;
using HunterPie.Core.API.Entities;
using HunterPie.UI.Architecture;
using System.Threading.Tasks;

namespace HunterPie.GUI.Parts.Account.ViewModels;

public class AccountSignFlowViewModel : ViewModel
{
    private int _selectedIndex;
    private string _signInUsername;
    private string _signInPassword;
    private bool _isLoggingIn;

    public int SelectedIndex { get => _selectedIndex; set => SetValue(ref _selectedIndex, value); }
    public string SignInUsername { get => _signInUsername; set => SetValue(ref _signInUsername, value); }
    public string SignInPassword { get => _signInPassword; set => SetValue(ref _signInPassword, value); }
    public bool IsLoggingIn { get => _isLoggingIn; set => SetValue(ref _isLoggingIn, value); }

    public async Task<bool> SignIn()
    {
        IsLoggingIn = true;
        LoginResponse? response = await PoogieApi.Login(new LoginRequest
        {
            Username = SignInUsername,
            Password = SignInPassword,
        });

        IsLoggingIn = false;

        return response is not null;
    }
}
