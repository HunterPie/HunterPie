using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Statistics.Views;
/// <summary>
/// Interaction logic for PartyMemberSummary.xaml
/// </summary>
public partial class PartyMemberDetailsView : UserControl, IEventDispatcher
{
    public event EventHandler<EventArgs> Click;

    public PartyMemberDetailsView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e) => this.Dispatch(Click);
}
