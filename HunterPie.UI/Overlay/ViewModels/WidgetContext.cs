using HunterPie.Core.Client.Configuration;
using HunterPie.UI.Architecture;
using HunterPie.UI.Overlay.Service;

namespace HunterPie.UI.Overlay.ViewModels;

#nullable enable
public sealed class WidgetContext : ViewModel
{
    public OverlayConfig OverlaySettings { get; }

    public WidgetViewModel ViewModel { get; }

    public IOverlayState State { get; }

    public WidgetContext(
        WidgetViewModel viewModel,
        OverlayConfig overlaySettings,
        IOverlayState state)
    {
        ViewModel = viewModel;
        OverlaySettings = overlaySettings;
        State = state;
    }
}