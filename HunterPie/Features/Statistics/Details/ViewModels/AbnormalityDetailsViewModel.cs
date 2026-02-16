using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Media;

namespace HunterPie.Features.Statistics.Details.ViewModels;

public class AbnormalityDetailsViewModel : ViewModel, ISectionControllable
{
    public string Name { get; set => SetValue(ref field, value); }
    public string Icon { get; set => SetValue(ref field, value); }
    public double UpTime { get; set => SetValue(ref field, value); }
    public bool IsToggled { get; set => SetValue(ref field, value); }

    public List<AxisSection> Activations { get; init; } = new();
    public Brush Color { get; set => SetValue(ref field, value); }
}