using HunterPie.Core.Logger;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using HunterPie.UI.Logger;
using System.Windows;
using HunterPie.UI.Settings;
using System.Windows.Controls;
using HunterPie.Core.Client.Configuration;
using HunterPie.GUI.Parts.Host;

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
            TestPopupWindow();
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
