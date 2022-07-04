using HunterPie.UI.Controls.Settings.ViewModel;
using System.Windows.Controls;
using HunterPie.UI.Controls.TextBox.Events;
using System.Windows;

namespace HunterPie.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SettingHost.xaml
    /// </summary>
    public partial class SettingHost : UserControl
    {
        public SettingHostViewModel ViewModel => (SettingHostViewModel)DataContext;

        public SettingHost()
        {
            InitializeComponent();
        }

        private void OnRealTimeSearch(object sender, SearchTextChangedEventArgs e) => ViewModel.SearchSetting(e.Text);
        private void OnLoaded(object sender, RoutedEventArgs e) => ViewModel.FetchVersion();
        private void OnUnloaded(object sender, RoutedEventArgs e) => ViewModel.UnhookEvents();
        private void OnExecuteUpdateClick(object sender, RoutedEventArgs e) => ViewModel.ExecuteRestart();
    }
}
