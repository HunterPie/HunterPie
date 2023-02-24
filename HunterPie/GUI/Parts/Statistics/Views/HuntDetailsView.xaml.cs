using HunterPie.GUI.Parts.Statistics.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Statistics.Views;
/// <summary>
/// Interaction logic for HuntDetailsView.xaml
/// </summary>
public partial class HuntDetailsView : UserControl
{
    // TODO: Turn this into view later
    private HuntDetailsViewModel ViewModel => (HuntDetailsViewModel)DataContext;

    public HuntDetailsView()
    {
        InitializeComponent();
    }

    private void OnPartyMemberClick(object sender, EventArgs e)
    {
        if (sender is PartyMemberDetailsView { DataContext: PartyMemberDetailsViewModel vm })
            ViewModel.FilterOnly(vm);
    }

    private void OnAbnormalityClick(object sender, EventArgs e)
    {
        if (sender is FrameworkElement { DataContext: AbnormalityDetailsViewModel vm })
        {
            if (!vm.IsToggled)
            {
                ViewModel.AddSections(vm.Sections);
                PART_Graph.Update();
            }
            else
                ViewModel.RemoveSections(vm.Sections);

            vm.IsToggled = !vm.IsToggled;

        }
    }
}
