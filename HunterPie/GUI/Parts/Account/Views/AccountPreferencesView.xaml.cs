using HunterPie.GUI.Parts.Account.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountPreferencesView.xaml
/// </summary>
public partial class AccountPreferencesView : UserControl
{
    private readonly AccountPreferencesViewModel _viewModel = new();

    public AccountPreferencesView()
    {
        InitializeComponent();
        DataContext = _viewModel;
    }

    private void OnLoad(object sender, RoutedEventArgs e) => _viewModel.FetchAccount();

    private void OnAvatarUploadClick(object sender, RoutedEventArgs e) => _viewModel.UploadAvatar();

    private void OnUnload(object sender, RoutedEventArgs e) => _viewModel.Dispose();
}
