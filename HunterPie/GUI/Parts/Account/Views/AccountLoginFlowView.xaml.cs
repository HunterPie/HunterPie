﻿using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;
using System.Windows.Input;

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

    private async void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        await ViewModel.SignIn();
    }
}
