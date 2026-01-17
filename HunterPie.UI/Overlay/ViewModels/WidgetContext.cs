using HunterPie.Core.Client.Configuration.Debug;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Service;

namespace HunterPie.UI.Overlay.ViewModels;

#nullable enable
public sealed class WidgetContext(
    WidgetViewModel viewModel,
    OverlayClientConfig overlaySettings,
    DevelopmentConfig developmentSettings,
    IOverlayState state) : ViewModel
{
    public OverlayClientConfig OverlaySettings { get; } = overlaySettings;

    public DevelopmentConfig DevelopmentSettings { get; } = developmentSettings;
    public WidgetViewModel ViewModel { get; set => SetValue(ref field, value); } = viewModel;

    public IOverlayState State { get; } = state;
    public double RenderTime { get; set => SetValue(ref field, value); }
}