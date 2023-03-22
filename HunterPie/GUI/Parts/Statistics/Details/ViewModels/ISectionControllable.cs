using LiveCharts.Wpf;
using System.Collections.Generic;

namespace HunterPie.GUI.Parts.Statistics.Details.ViewModels;

public interface ISectionControllable
{
    public bool IsToggled { get; set; }

    public List<AxisSection> Activations { get; }
}