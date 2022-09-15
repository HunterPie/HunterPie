using HunterPie.Core.API;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Account.ViewModels
{
    public class AccountSignFlowViewModel : ViewModel
    {
        private int _selectedIndex;
        private string _signInEmail;
        private string _signInPassword;
        private bool _isLoggingIn;

        public int SelectedIndex { get => _selectedIndex; set { SetValue(ref _selectedIndex, value); } }
        public string SignInEmail { get => _signInEmail; set { SetValue(ref _signInEmail, value); } }
        public string SignInPassword { get => _signInPassword; set { SetValue(ref _signInPassword, value); } }
        public bool IsLoggingIn { get => _isLoggingIn; set { SetValue(ref _isLoggingIn, value); } }

        public async void SignIn()
        {
            
        }
    }
}
