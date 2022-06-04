using HunterPie.UI.Architecture;
using System;
using System.Windows.Input;

namespace HunterPie.GUI.Parts
{
    /// <summary>
    /// Interaction logic for HeaderBar.xaml
    /// </summary>
    public partial class HeaderBar : View<HeaderBarViewModel>
    {

        public HeaderBar()
        {
            InitializeComponent();

            ViewModel.FetchSupporterStatus();
        }

        private void OnCloseButtonClick(object sender, EventArgs e) => ViewModel.CloseApplication();
        private void OnMinimizeButtonClick(object sender, EventArgs e) => ViewModel.MinimizeApplication();
        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e) => ViewModel.DragApplication();
    }
}
