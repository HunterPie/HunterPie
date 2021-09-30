using HunterPie.UI.Architecture.Extensions;
using System.ComponentModel;
using System.Windows;

namespace HunterPie.UI.Overlay.Components
{
    /// <summary>
    /// Interaction logic for WidgetBase.xaml
    /// </summary>
    public partial class WidgetBase : Window, INotifyPropertyChanged
    {
        private object _widget;
        
        public object Widget
        {
            get => _widget;
            internal set
            {
                if (value != _widget)
                {
                    _widget = value;
                    this.N(PropertyChanged);
                }
            }
        }

        public WidgetBase()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
