using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using System;

namespace HunterPie.UI.Overlay.Widgets.Metrics.View;

/// <summary>
/// Interaction logic for TelemetricsView.xaml
/// </summary>
public partial class TelemetricsView
{
    private TelemetricsViewModel ViewModel => (TelemetricsViewModel)DataContext;

    public TelemetricsView()
    {
        InitializeComponent();
    }

    private void OnGCClick(object sender, EventArgs e) => ViewModel.ExecuteGarbageCollector();
}