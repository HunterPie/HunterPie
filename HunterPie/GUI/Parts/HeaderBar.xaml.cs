using HunterPie.UI.Architecture;
using System.Windows.Controls;
using System;
using System.Windows.Input;
using HunterPie.Core.Logger;

namespace HunterPie.GUI.Parts
{
    /// <summary>
    /// Interaction logic for HeaderBar.xaml
    /// </summary>
    public partial class HeaderBar : UserControl, IView<HeaderBarViewModel>
    {
        private readonly HeaderBarViewModel _viewModel = new HeaderBarViewModel();
        public HeaderBarViewModel Model => _viewModel;

        public HeaderBar()
        {
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void OnCloseButtonClick(object sender, EventArgs e) => _viewModel.CloseApplication();
        private void OnMinimizeButtonClick(object sender, EventArgs e) => _viewModel.MinimizeApplication();

        private void OnLeftMouseDown(object sender, MouseButtonEventArgs e) => _viewModel.DragApplication();
    }
}
