using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay;
using System;
using HunterPie.Core.Logger;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.Internal;
using HunterPie.UI.Overlay.Widgets.Damage.View;
using System.Windows.Input;
using System.Diagnostics;
using HunterPie.UI.Overlay.Widgets.Monster.Views;

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

            base.OnClosing(e);
        }

        private async void OnInitialized(object sender, EventArgs e)
        {
            InitializerManager.InitializeGUI();
            WidgetManager.Register(new MeterView());
            WidgetManager.Register(new MonstersView());
            WidgetManager.Register(new AbnormalityBarView());
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) && e.KeyboardDevice.IsKeyDown(Key.R))
            {
                Process.Start(typeof(MainWindow).Assembly.Location.Replace(".dll", ".exe"));
                Application.Current.Shutdown();
            }
        }
    }
}
