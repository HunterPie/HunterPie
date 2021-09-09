using HunterPie.UI.Controls.Settings.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings
{
    /// <summary>
    /// Interaction logic for SettingHost.xaml
    /// </summary>
    public partial class SettingHost : UserControl
    {

        private readonly ObservableCollection<ISettingElement> _elements = new ObservableCollection<ISettingElement>();
        public ObservableCollection<ISettingElement> Elements => _elements;

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
    }
}
