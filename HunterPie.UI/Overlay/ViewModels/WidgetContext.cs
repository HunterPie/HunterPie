using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Service;

namespace HunterPie.UI.Overlay.ViewModels;

#nullable enable
public sealed class WidgetContext : ViewModel
{
    public OverlayClientConfig OverlaySettings { get; }

    public DevelopmentConfig DevelopmentSettings { get; }

    private WidgetViewModel _viewModel;
    public WidgetViewModel ViewModel { get => _viewModel; set => SetValue(ref _viewModel, value); }

    public IOverlayState State { get; }

    private double _renderTime;
    public double RenderTime { get => _renderTime; set => SetValue(ref _renderTime, value); }

    public WidgetContext(
        WidgetViewModel viewModel,
        OverlayClientConfig overlaySettings,
        DevelopmentConfig developmentSettings,
        IOverlayState state)
    {
        _viewModel = viewModel;
        OverlaySettings = overlaySettings;
        DevelopmentSettings = developmentSettings;
        State = state;
    }
}