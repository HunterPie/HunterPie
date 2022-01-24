using HunterPie.Core.Architecture;
using HunterPie.UI.Controls.Settings.ViewModel;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using HunterPie.UI.Controls.TextBox.Events;
using HunterPie.Core.Logger;
using System.Windows.Media.Animation;
using System.Windows;
using System;

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
    }
}
