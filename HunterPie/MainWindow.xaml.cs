using HunterPie.Core.Logger;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using HunterPie.UI.Logger;
using System.Windows;
using System.Windows.Controls;

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
    }
}
