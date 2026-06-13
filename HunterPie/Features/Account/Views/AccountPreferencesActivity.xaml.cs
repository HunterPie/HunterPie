using HunterPie.Features.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.Features.Account.Views;

/// <summary>
/// Interaction logic for AccountPreferencesActivity.xaml
/// </summary>
public partial class AccountPreferencesActivity : Activity
{
    public AccountPreferencesActivity()
    {
        InitializeComponent();
    }

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AccountPreferencesViewModel vm)
            return;

        vm.UploadAvatar();
    }
}