using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Account.Views;
/// <summary>
/// Interaction logic for RequestAccountVerificationFlowView.xaml
/// </summary>
public partial class RequestAccountVerificationFlowView : View<AccountVerificationResendFlowViewModel>
{
    public RequestAccountVerificationFlowView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToLoginFlow();

    private async void OnResendVerificationButtonClick(object sender, RoutedEventArgs e) => await ViewModel.RequestAccountVerification();
}