using HunterPie.GUI.Parts.Notifications.ViewModels;
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

namespace HunterPie.GUI.Parts.Notifications.Views
{
    /// <summary>
    /// Interaction logic for NotificationsPanelView.xaml
    /// </summary>
    public partial class NotificationsPanelView : UserControl
    {
        private NotificationsPanelViewModel ViewModel => (NotificationsPanelViewModel)DataContext;

        public NotificationsPanelView()
        {
            DataContext = new NotificationsPanelViewModel();
            InitializeComponent();
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.FetchNotifications();
        }
    }
}
