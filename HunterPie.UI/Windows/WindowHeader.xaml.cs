using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HunterPie.UI.Windows
{
    /// <summary>
    /// Interaction logic for WindowHeader.xaml
    /// </summary>
    public partial class WindowHeader : UserControl
    {
        public Window Owner { get; private set; }
        public object Container
        {
            get { return (object)GetValue(ContainerProperty); }
            set { SetValue(ContainerProperty, value); }
        }
        public static readonly DependencyProperty ContainerProperty =
            DependencyProperty.Register("Container", typeof(object), typeof(WindowHeader));

        public WindowHeader()
        {
            InitializeComponent();
        }

        
        private void OnCloseButtonClick(object sender, EventArgs e) => Owner.Close();

        private void OnMinimizeButtonClick(object sender, EventArgs e) => Owner.WindowState = WindowState.Minimized;

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e) => Owner.DragMove();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Owner = Window.GetWindow(this);
        }
    }
}
