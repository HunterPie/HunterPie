using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Settings;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Classes;
using HunterPie.UI.Overlay.Widgets.Classes.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class ClassWidgetInitializer : IWidgetInitializer
{
    private readonly IOverlay _overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise |
        GameProcessType.MonsterHunterWorld;

    public ClassWidgetInitializer(IOverlay overlay)
    {
        _overlay = overlay;
    }

    public Task LoadAsync(IContext context)
    {
        IWidgetSettings config = ClientConfigHelper.DeferOverlayConfig(context.Process.Type, it => it.LongSwordWidget);

        var viewModel = new ClassViewModel(config);
        _handler = new ClassWidgetContextHandler(
            context: context,
            viewModel: viewModel
        );

        _view = _overlay.Register(viewModel);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}