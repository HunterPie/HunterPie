using HunterPie.GUI.Parts.Account.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Account.Views.Components;
/// <summary>
/// Interaction logic for AccountDetailsView.xaml
/// </summary>
public partial class AccountDetailsView : UserControl
{
    private AccountPreferencesViewModel ViewModel => (AccountPreferencesViewModel)DataContext;

    public AccountDetailsView()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e) => ViewModel.UploadAvatar();
}
