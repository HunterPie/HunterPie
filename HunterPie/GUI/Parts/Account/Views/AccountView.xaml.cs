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

        private void OnAvatarClick(object sender, EventArgs e)
        {
            
            if (sender is UserControl obj)
            {
                var wasFocused = Keyboard.FocusedElement;

                if (wasFocused == sender)
                    Keyboard.ClearFocus();
                else
                    obj.Focus();
            }

        }

        private void OnAvatarGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = true;
        private void OnAvatarLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) => ViewModel.IsAvatarClicked = false;
    }
}
