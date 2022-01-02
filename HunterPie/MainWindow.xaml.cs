using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster;
using System;
using HunterPie.Core.Logger;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.UI.Overlay.Widgets.Metrics.View;
using HunterPie.Internal;
using HunterPie.UI.Overlay.Widgets.Damage.View;

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
        }

    }
}
