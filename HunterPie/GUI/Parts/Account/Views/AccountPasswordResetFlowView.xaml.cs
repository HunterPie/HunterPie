using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountPasswordResetFlowView.xaml
/// </summary>
public partial class AccountPasswordResetFlowView : View<AccountPasswordResetFlowViewModel>
{
    public AccountPasswordResetFlowView()
    {
        InitializeComponent();
    }

    private async void OnSendCodeButtonClick(object sender, RoutedEventArgs e) => await ViewModel.RequestResetCodeAsync();

    private async void OnPasswordChangeButtonClick(object sender, RoutedEventArgs e) => await ViewModel.ChangePassword();

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToLoginFlow();
}
