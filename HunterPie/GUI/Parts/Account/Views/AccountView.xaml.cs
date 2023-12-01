using HunterPie.Features.Account;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountView.xaml
/// </summary>
public partial class AccountView : View<AccountViewModel>
{
    public AccountView()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        ViewModel.FetchAccountDetails();
    }

    private void OnAvatarGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = true;
    private void OnAvatarLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = false;

    private void OnAvatarClick(object sender, RoutedEventArgs e)
    {
        if (sender is UserControl obj)
        {
            IInputElement wasFocused = Keyboard.FocusedElement;

            if (wasFocused == sender)
                Keyboard.ClearFocus();
            else
                _ = obj.Focus();
        }

        ViewModel.OpenAccountPreferences();
    }

    private void OnLoginClick(object sender, RoutedEventArgs e) => AccountNavigationService.NavigateToSignIn();
    private void OnRegisterClick(object sender, RoutedEventArgs e) => AccountNavigationService.NavigateToSignUp();
    private void OnLogoutClick(object sender, RoutedEventArgs e) => ViewModel.SignOut();
    private void OnSettingsClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToSettings();
}
