using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.GUI.Parts.Account.Views;
/// <summary>
/// Interaction logic for AccountLoginFlowView.xaml
/// </summary>
public partial class AccountLoginFlowView : View<AccountLoginFlowViewModel>
{
    public AccountLoginFlowView()
    {
        InitializeComponent();
    }

    private async void OnSignInClick(object sender, EventArgs e)
    {
        if (!await ViewModel.SignIn())
            return;
    }
}
