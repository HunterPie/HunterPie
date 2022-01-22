using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay;
using System;
using HunterPie.Core.Logger;
using HunterPie.Internal;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using System.Windows.Input;
using System.Diagnostics;
using HunterPie.UI.Overlay.Widgets.Monster.Views;
using HunterPie.Core.Client;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModel;
using HunterPie.Internal.Tray;
using HunterPie.Update.Presentation;
using HunterPie.Update;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Log.Info("Initializing HunterPie GUI");
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            NativeDialogResult result = DialogManager.Info("Confirmation", "Are you sure you want to exit HunterPie?", NativeDialogButtons.Accept | NativeDialogButtons.Cancel);  
            
            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }
            InitializerManager.Unload();

            base.OnClosing(e);
        }

        private async void OnInitialized(object sender, EventArgs e)
        {
            InitializerManager.InitializeGUI();
            InitializeDebugWidgets();
            
            await HandleAutoUpdate();

            Show();
            SetupTrayIcon();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
                Restart();
        }

        private void InitializeDebugWidgets()
        {
            if (ClientConfig.Config.Debug.MockBossesWidget)
                WidgetManager.Register(
                    new MonstersView()
                    {
                        DataContext = new MockMonstersViewModel()
                    }
                );

            if (ClientConfig.Config.Debug.MockDamageWidget)
                WidgetManager.Register(
                    new MeterView()
                    {
                        DataContext = new MockMeterViewModel()
                    }
                );
        }

        private void SetupTrayIcon()
        {
            TrayService.AddDoubleClickHandler((_, __) =>
            {
                Show();
                WindowState = WindowState.Normal;
                Focus();
            });

            TrayService.AddItem("Show")
                .Click += (_, __) =>
                {
                    Show();
                    WindowState = WindowState.Normal;
                    Focus();
                };

            TrayService.AddItem("Close")
                .Click += (_, __) => {
                    Close();
                };
        }

        // TODO: Implement all this logic in another class
        private async Task HandleAutoUpdate()
        {
            Hide();
            UpdateViewModel vm = new()
            {
                State = "Initializing HunterPie"
            };
            UpdateView view = new()
            {
                DataContext = vm
            };
            view.Show();

            UpdateService service = new();
            service.CleanupOldFiles();

            vm.State = "Checking for latest version...";
            Version latest = await service.GetLatestVersion();

            if (latest is null || ClientInfo.IsVersionGreaterOrEq(latest))
            {
                view.Close();
                return;
            }

            var result = DialogManager.Warn(
                    "Update",
                    "There's a new version of HunterPie.\nDo you want to update now?",
                    NativeDialogButtons.Accept | NativeDialogButtons.Reject);

            Dictionary<string, string> localFiles = await service.IndexAllFilesRecursively(ClientInfo.ClientPath);

            if (result != NativeDialogResult.Accept)
            {
                view.Close();
                return;
            }

            vm.State = "New version found";

            vm.State = "Downloading package";
            await service.DownloadZip((_, args) => {
                vm.DownloadedBytes = args.BytesReceived;
                vm.TotalBytes = args.TotalBytesToReceive;
            });

            vm.State = "Extracting package...";
            service.ExtractZip();
            Dictionary<string, string> remoteFiles = await service.IndexAllFilesRecursively(ClientInfo.GetPathFor(@"temp/HunterPie"));

            vm.State = "Replacing old files";
            service.ReplaceOldFiles(localFiles, remoteFiles);

            view.Close();
            Restart();
        }

        private void Restart()
        {
            Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ".exe"));
            Application.Current.Shutdown();
        }
    }
}
