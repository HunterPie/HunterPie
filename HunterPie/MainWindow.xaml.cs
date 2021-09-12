using HunterPie.Core.Logger;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using System.Windows;
using HunterPie.Core.Domain.Dialog;

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
        }

        private void InitializeSideMenu()
        {
            ISideBar menu = new DefaultSideBar();
            SideBarContainer.SetMenu(menu);
        }

        private void TestPopupWindow()
        {
            TestWindow t = new();
            t.Show();
        }
    }
}
