using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class StatusDetailsViewModel : ViewModel, ISectionControllable
{
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;
    public double UpTime { get; set => SetValue(ref field, value); }
    public bool IsToggled { get; set => SetValue(ref field, value); }

    public List<AxisSection> Activations { get; init; } = new();
    public Brush Color { get; set => SetValue(ref field, value); } = Brushes.Transparent;
}