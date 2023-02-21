using HunterPie.Features.Statistics.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Statistics.Views;
/// <summary>
/// Interaction logic for HuntSummaryView.xaml
/// </summary>
public partial class HuntSummaryView : UserControl
{
    // TODO: Turn this into view later
    private HuntSummaryViewModel ViewModel => (HuntSummaryViewModel)DataContext;

    public HuntSummaryView()
    {
        InitializeComponent();
    }

    private void OnPartyMemberClick(object sender, EventArgs e)
    {
        if (sender is PartyMemberSummaryView view && view.DataContext is PartyMemberSummaryViewModel vm)
            ViewModel.FilterOnly(vm);
    }

    private void OnAbnormalityClick(object sender, EventArgs e)
    {
        if (sender is FrameworkElement view && view.DataContext is AbnormalitySummaryViewModel vm)
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
