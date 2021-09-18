using HunterPie.GUI.Parts.Sidebar.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows.Controls;
using System;

namespace HunterPie.GUI.Parts.Sidebar
{
    /// <summary>
    /// Interaction logic for SideBarElement.xaml
    /// </summary>
    public partial class SideBarElement : UserControl
    {
        public ISideBarElement Model => (ISideBarElement)DataContext;

        public SideBarElement()
        {
            InitializeComponent();
        }

        private void OnButtonClick(object sender, EventArgs e) => Model.ExecuteOnClick();
    }
}
