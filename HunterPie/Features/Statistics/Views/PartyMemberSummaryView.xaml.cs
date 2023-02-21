using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Statistics.Views;
/// <summary>
/// Interaction logic for PartyMemberSummary.xaml
/// </summary>
public partial class PartyMemberSummaryView : UserControl, IEventDispatcher
{
    public event EventHandler<EventArgs> Click;

    public PartyMemberSummaryView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e) => this.Dispatch(Click);
}
