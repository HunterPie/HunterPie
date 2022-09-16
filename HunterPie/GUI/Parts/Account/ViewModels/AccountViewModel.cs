using HunterPie.GUI.Parts.Account.Views;
using HunterPie.GUI.Parts.Host;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Account.ViewModels
{
    public class AccountViewModel : ViewModel
    {
        private bool _isAvatarClicked;
        private string _avatarUrl;
        private string _username;
        private bool _isLoggedIn;

        public bool IsAvatarClicked { get => _isAvatarClicked; set => SetValue(ref _isAvatarClicked, value); }
        public string AvatarUrl { get => _avatarUrl; set => SetValue(ref _avatarUrl, value); }
        public string Username { get => _username; set => SetValue(ref _username, value); }
        public bool IsLoggedIn { get => _isLoggedIn; set => SetValue(ref _isLoggedIn, value); }

        public void OpenAccountPreferences()
        {
            var preferences = new AccountPreferencesView();
            MainHost.SetMain(preferences);
        }
    }
}
