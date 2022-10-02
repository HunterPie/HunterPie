using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Account.Views;
/// <summary>
/// Interaction logic for AccountRegisterFlowView.xaml
/// </summary>
public partial class AccountRegisterFlowView : View<AccountRegisterFlowViewModel>
{
    public AccountRegisterFlowView()
    {
        InitializeComponent();
    }

    private void OnRegisterClick(object sender, RoutedEventArgs e) => ViewModel.SignUp();
}
