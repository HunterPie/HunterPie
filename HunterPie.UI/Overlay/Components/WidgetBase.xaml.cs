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
        private UIElement _widget;
        
        public UIElement Widget
        {
            get => _widget;
            set
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
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
