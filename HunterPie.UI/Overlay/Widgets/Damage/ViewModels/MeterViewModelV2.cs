using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class MeterViewModelV2 : MeterViewModel
{
    private double _maxPlotValue = 5;
    public double MaxPlotValue { get => _maxPlotValue; set => SetValue(ref _maxPlotValue, value); }

    public MeterViewModelV2(DamageMeterWidgetConfig config) : base(config)
    {
    }
}