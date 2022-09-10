using HunterPie.Core.Logger;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.GUI.Parts.Account.Views
{
    /// <summary>
    /// Interaction logic for AccountView.xaml
    /// </summary>
    public partial class AccountView : View<AccountViewModel>
    {
        public AccountView()
        {
            InitializeComponent();
        }

        private void OnAvatarGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = true;
        private void OnAvatarLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = false;

        private void OnAvatarClick(object sender, RoutedEventArgs e)
        {
            if (sender is UserControl obj)
            {
                var wasFocused = Keyboard.FocusedElement;

                if (wasFocused == sender)
                    Keyboard.ClearFocus();
                else
                    obj.Focus();
            }

            ViewModel.OpenAccountPreferences();
        }

        private void OnLoginClick(object sender, RoutedEventArgs e)
        {
            Log.Info("Test");
        }

        private void OnRegisterClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
