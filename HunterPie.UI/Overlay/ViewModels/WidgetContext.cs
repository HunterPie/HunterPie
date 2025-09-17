using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Service;

namespace HunterPie.UI.Overlay.ViewModels;

#nullable enable
public sealed class WidgetContext : ViewModel
{
    public OverlayClientConfig OverlaySettings { get; }

    private WidgetViewModel _viewModel;
    public WidgetViewModel ViewModel { get => _viewModel; set => SetValue(ref _viewModel, value); }

    public IOverlayState State { get; }

    public WidgetContext(
        WidgetViewModel viewModel,
        OverlayClientConfig overlaySettings,
        IOverlayState state)
    {
        _viewModel = viewModel;
        OverlaySettings = overlaySettings;
        State = state;
    }
}