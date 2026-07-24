using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Utils;
using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Header.ViewModels;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace HunterPie.UI.Header.Views;

/// <summary>
/// Interaction logic for AccountMenuView.xaml
/// </summary>
[View<AccountMenuViewModel>]
public partial class AccountMenuView
{
    public AccountMenuView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        ViewModel.IsOpen = !ViewModel.IsOpen;
    }

    private void OnDropDownChanged(object? sender, DataTransferEventArgs e)
    {
        if (ViewModel.IsOpen)
            Mouse.Capture(this, CaptureMode.SubTree);
        else
            Mouse.Capture(null);
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        var parentWindow = Window.GetWindow(this);

        if (parentWindow is null)
            return;

        parentWindow.Deactivated += (_, __) => ViewModel.IsOpen = false;
    }

    private void OnMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (Mouse.Captured is null)
            return;

        Point buttonDistance = e.GetPosition(this);
        bool isWithinBounds = buttonDistance.IsWithinBounds(this);

        if (isWithinBounds)
            return;

        ViewModel.IsOpen = false;
    }

    private void OnSignInButtonClick(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenSignInScreen();
    }

    private void OnSignOutClick(object sender, RoutedEventArgs e)
    {
        ViewModel.SignOut();
    }

    private void OnAccountDetailsClick(object sender, RoutedEventArgs e)
    {
        ViewModel.OpenAccountDetails();
    }

    private async void OnAccountSettingsClick(object sender, RoutedEventArgs e)
    {
        await ViewModel.OpenAccountSettingsAsync();
    }
}