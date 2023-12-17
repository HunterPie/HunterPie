using HunterPie.UI.Architecture;

namespace HunterPie.UI.Header.ViewModels;

internal class AccountMenuViewModel : ViewModel
{
    private bool _isLoading;
    public bool IsLoading { get => _isLoading; set => SetValue(ref _isLoading, value); }

    private string _avatarUrl = string.Empty;
    public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }

    private string _username = string.Empty;
    public string Username { get => _username; set => SetValue(ref _username, value); }

    private bool _isLoggedIn;
    public bool IsLoggedIn { get => _isLoggedIn; set => SetValue(ref _isLoggedIn, value); }

    private bool _isOpen;
    public bool IsOpen { get => _isOpen; set => SetValue(ref _isOpen, value); }

    public void OpenSignInScreen()
    {

    }
}