using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using System;
using HunterPie.Core.Logger;
using HunterPie.Internal;
using System.Windows.Input;
using HunterPie.Core.Client;
using HunterPie.Internal.Tray;
using System.Windows.Media.Animation;
using HunterPie.Features.Debug;
using HunterPie.Core.API;
using HunterPie.Core.Utils;
using HunterPie.GUI.ViewModels;
using HunterPie.GUI.Parts.Host;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            DataContext = new MainViewModel();

            Log.Info("Initializing HunterPie GUI");

            Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = (int)ClientConfig.Config.Client.RenderFramePerSecond.Current });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
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

            AsyncHelper.RunSync(PoogieApi.EndSession);

            base.OnClosing(e);
        }

        private void OnInitialized(object sender, EventArgs e)
        {
            InitializerManager.InitializeGUI();
            
            InitializeDebugWidgets();
                       
            SetupTrayIcon();
            SetupMainNavigator();
        }

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
            DoubleAnimation shrinkAnimation = new DoubleAnimation(1.5, 1, TimeSpan.FromMilliseconds(200))
            {
                EasingFunction = new QuarticEase()
            };
            DoubleAnimation opacityAnimation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(500))
            {
                EasingFunction = new SineEase()
            };
            MainHost.Instance.PropertyChanged += (_, __) =>
            {
                PART_ContentPresenter.BeginAnimation(FrameworkElement.OpacityProperty, opacityAnimation);
                PART_ContentPresenter.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
                PART_ContentPresenter.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkAnimation);
            };
        }

        private void OnTrayShowClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            Focus();
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
    }
}
