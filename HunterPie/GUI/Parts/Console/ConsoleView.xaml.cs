using HunterPie.UI.Architecture;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Console
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView : UserControl, IView<ConsoleViewModel>
    {
        private ConsoleViewModel viewModel = new ConsoleViewModel();
        public ConsoleViewModel Model => viewModel;

        public ConsoleView()
        {
            InitializeComponent();
            DataContext = Model;
        }

        
    }
}
