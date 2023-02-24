using HunterPie.UI.Architecture;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.Windows.Media;

namespace HunterPie.GUI.Parts.Statistics.ViewModels;
public class AbnormalityDetailsViewModel : ViewModel
{
    private string _id;
    private double _uptime;
    private bool _isToggled;
    private Brush _color;

    public string Id { get => _id; set => SetValue(ref _id, value); }
    public double Uptime { get => _uptime; set => SetValue(ref _uptime, value); }
    public bool IsToggled { get => _isToggled; set => SetValue(ref _isToggled, value); }
    public List<AxisSection> Sections { get; } = new();
    public Brush Color { get => _color; set => SetValue(ref _color, value); }
}
