using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay.Components;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            InitializeSideMenu();
            //TestPopupWindow();
        }

        private void InitializeSideMenu()
        {
            ISideBar menu = new DefaultSideBar();

            menu.Menu[0].ExecuteOnClick();
            
            SideBarContainer.SetMenu(menu);
        }

        private void TestPopupWindow()
        {
            WidgetBase widget = new WidgetBase();
            widget.Show();
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
    }
}
