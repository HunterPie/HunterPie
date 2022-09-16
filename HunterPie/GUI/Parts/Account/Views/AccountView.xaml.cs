using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
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
    public partial class AccountView : View<AccountViewModel>, IEventDispatcher
    {
        public AccountView()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> OnSignInClicked;
        public event EventHandler<EventArgs> OnSignUpClicked;

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

        private void OnLoginClick(object sender, RoutedEventArgs e) => this.Dispatch(OnSignInClicked);
        private void OnRegisterClick(object sender, RoutedEventArgs e) => this.Dispatch(OnSignUpClicked);
    }
}
