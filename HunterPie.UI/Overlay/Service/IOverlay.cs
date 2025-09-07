using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;

namespace HunterPie.UI.Overlay.Service;

public interface IOverlay
{
    public WidgetView Register(WidgetViewModel viewModel);

    public void Unregister(WidgetView widget);
}