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
        // TODO: Put all this data in the ViewModel
        private readonly ObservableCollection<ISettingElement> _elements = new ObservableCollection<ISettingElement>();
        public ObservableCollection<ISettingElement> Elements => _elements;
        public Observable<int> CurrentTabIndex { get; } = 0;

        public SettingHost()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void AddTab(ISettingElement element)
        {
            _elements.Add(element);
        }

        public void AddTab(params ISettingElement[] elements)
        {
            foreach (ISettingElement el in elements)
                AddTab(el);
        }

        private void OnRealTimeSearch(object sender, SearchTextChangedEventArgs e)
        {
            ISettingElement tab = Elements[CurrentTabIndex];

            foreach (ISettingElementType field in tab.Elements)
                field.Match = Regex.IsMatch(field.Name, e.Text, RegexOptions.IgnoreCase) || e.Text.Length == 0;
        }
    }
}
