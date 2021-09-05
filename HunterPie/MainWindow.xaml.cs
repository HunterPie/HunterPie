using HunterPie.GUI.Parts.Sidebar;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
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

        private void OnConsoleLoaded(object sender, RoutedEventArgs e)
        {
            ItemsControl console = (ItemsControl)sender;
            console.ItemsSource = NativeLogger.viewModel;
        }

        private void InitializeSideMenu()
        {
            SideBarContainer.Add(
                    new ConsoleSideBarElementViewModel(),
                    new SettingsSideBarElementViewModel(),
                    new PluginsSideBarElementViewModel(),
                    new PatchNotesSideBarElementViewModel()
                );
        }
    }
}
