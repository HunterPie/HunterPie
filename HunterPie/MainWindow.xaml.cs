using HunterPie.Core.Logger;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using HunterPie.UI.Logger;
using System.Windows;
using HunterPie.UI.Settings;
using System.Windows.Controls;
using HunterPie.Core.Client.Configuration;

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
            TestSettingsBuilder();
        }

        private void InitializeSideMenu()
        {
            ISideBar menu = new DefaultSideBar();
            SideBarContainer.SetMenu(menu);
        }

        private void TestSettingsBuilder()
        {
            var builder = new SettingsConstructor();
            var x = builder.Build(new Config());
            PART_Setting.AddTab(x);
        }
    }
}
