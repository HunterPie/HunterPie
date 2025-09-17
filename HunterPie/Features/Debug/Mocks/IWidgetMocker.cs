using HunterPie.Core.Architecture;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;

namespace HunterPie.Features.Debug.Mocks;

internal interface IWidgetMocker
{
    public Observable<bool> Setting { get; }

    public WidgetView Mock(IOverlay overlay);
}