using HunterPie.Core.Client;
using HunterPie.Core.Domain.Dialog;
using HunterPie.Core.Logger;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Config;
using HunterPie.Features.Account.UseCase;
using HunterPie.Features.Debug;
using HunterPie.Features.Notification.ViewModels;
using HunterPie.GUI.Parts.Account.Views;
using HunterPie.GUI.Parts.Host;
using HunterPie.GUI.ViewModels;
using HunterPie.Internal;
using HunterPie.Internal.Tray;
using HunterPie.UI.Controls.Notfication;
using HunterPie.Usecases;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly RemoteAccountConfigService _remoteConfigService = new();
    private MainViewModel ViewModel => (MainViewModel)DataContext;

    public MainWindow()
    {
        DataContext = new MainViewModel();

        Log.Info("Initializing HunterPie GUI");

        Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = (int)ClientConfig.Config.Client.RenderFramePerSecond.Current });
    }

    protected override async void OnClosing(CancelEventArgs e)
    {
        ConfigManager.SaveAll();

        if (!ClientConfig.Config.Client.EnableSeamlessShutdown)
        {
            NativeDialogResult result = DialogManager.Info(
                Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='CONFIRMATION_TITLE_STRING']"),
                Localization.QueryString("//Strings/Client/Dialogs/Dialog[@Id='EXIT_CONFIRMATION_DESCRIPTION_STRING']"),
                NativeDialogButtons.Accept | NativeDialogButtons.Cancel
            );

            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }
        }

        e.Cancel = true;

        await Dispatcher.InvokeAsync(Hide);

        await _remoteConfigService.UploadClientConfig();

        InitializerManager.Unload();

        await Dispatcher.InvokeAsync(() => Application.Current.Shutdown(0));
    }

    private async void OnInitialized(object sender, EventArgs e)
    {
        CheckIfHunterPiePathIsSafe();

        InitializerManager.InitializeGUI();

        InitializeDebugWidgets();

        SetupTrayIcon();
        SetupMainNavigator();
        SetupAccountEvents();

        await SetupPromoViewAsync();
    }

    private async Task SetupPromoViewAsync() => ViewModel.ShouldShowPromo = await AccountPromotionalUseCase.ShouldShow();

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
            App.Restart();
    }

    private void InitializeDebugWidgets() => DebugWidgets.MockIfNeeded();

    private void SetupTrayIcon()
    {
        TrayService.AddDoubleClickHandler(OnTrayShowClick);

        TrayService.AddItem("Show")
            .Click += OnTrayShowClick;

        TrayService.AddItem("Close")
            .Click += OnTrayCloseClick;
    }

    private void SetupMainNavigator()
    {
        var shrinkAnimation = new DoubleAnimation(1.5, 1, TimeSpan.FromMilliseconds(200))
        {
            EasingFunction = new QuarticEase()
        };

        var opacityAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500))
        {
            EasingFunction = new SineEase()
        };

        MainApplicationNavigator.Instance.PropertyChanged += (_, __) =>
        {
            PART_ContentPresenter.BeginAnimation(FrameworkElement.OpacityProperty, opacityAnimation);
            PART_ContentPresenter.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
            PART_ContentPresenter.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkAnimation);
        };
    }

    private void SetupAccountEvents()
    {
        AccountNavigationService.OnNavigateToSignIn += (_, __) => CreateSignFlowView(true);
        AccountNavigationService.OnNavigateToSignUp += (_, __) => CreateSignFlowView(false);
    }

    private void CreateSignFlowView(bool isLoggingIn)
    {
        AccountSignFlowView view = new();
        view.ViewModel.SelectedIndex = isLoggingIn ? 0 : 1;
        view.OnFormClose += OnSignFormClose;
        PART_SigninView.Content = view;
    }

    private void OnSignFormClose(object sender, EventArgs e)
    {
        if (sender is AccountSignFlowView view)
        {
            view.OnFormClose -= OnSignFormClose;
            PART_SigninView.Content = null;
        }
    }

    private void OnTrayShowClick(object sender, EventArgs e)
    {
        Show();
        WindowState = WindowState.Normal;
        _ = Focus();
    }

    private void OnTrayCloseClick(object sender, EventArgs e) => Close();

    private void OnStartGameClick(object sender, EventArgs e) => Steam.RunGameBy(ClientConfig.Config.Client.DefaultGameType);

    private void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (IsMouseOverElement(e, PART_HeaderBar))
            return;

        if (!IsMouseOverElement(e, PART_NotificationsPanel))
            PART_HeaderBar.ViewModel.IsNotificationsToggled = false;
    }

    private bool IsMouseOverElement(MouseEventArgs args, FrameworkElement element)
    {
        Point points = args.GetPosition(element);
        HitTestResult result = VisualTreeHelper.HitTest(element, points);

        return result != null;
    }

    private void CheckIfHunterPiePathIsSafe()
    {
        bool isSafe = VerifyHunterPiePathUseCase.Invoke();

        if (isSafe)
            return;

        _ = DialogManager.Warn(
            "Unsafe path",
            "It looks like you're executing HunterPie directly from the zip file. Please extract it first before running the client.",
            NativeDialogButtons.Accept
        );

        Application.Current.Shutdown();
    }

    private void OnNotificationClick(object sender, RoutedEventArgs e)
    {
        if (sender is Push { DataContext: AppNotificationViewModel vm })
            vm.IsVisible = false;
    }
}
