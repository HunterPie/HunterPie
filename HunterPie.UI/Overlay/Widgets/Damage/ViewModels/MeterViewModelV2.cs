using HunterPie.Core.Client.Configuration.Overlay;

namespace HunterPie.UI.Overlay.Widgets.Damage.ViewModels;

public class MeterViewModelV2(DamageMeterWidgetConfig config) : MeterViewModel(config)
{
    public double MaxPlotValue { get; set => SetValue(ref field, value); } = 5;
}