using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Views;
using System.Windows;

namespace HunterPie.Features.Account.Views;

/// <summary>
/// Interaction logic for AccountPreferencesActivity.xaml
/// </summary>
[View<AccountPreferencesViewModel>]
public partial class AccountPreferencesActivity : Activity
{
    public AccountPreferencesActivity()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e)
    {
        ViewModel.UploadAvatar();
    }
}