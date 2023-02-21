using HunterPie.GUI.Parts.Sidebar.Service;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using System;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Sidebar;

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

    private void OnButtonClick(object sender, EventArgs e) => SideBarService.Navigate(Model);
}
