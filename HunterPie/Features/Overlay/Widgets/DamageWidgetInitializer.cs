using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Damage.Controllers;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class DamageWidgetInitializer : IWidgetInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly IOverlay _overlay;

    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise
        | GameProcessType.MonsterHunterWorld
        | GameProcessType.MonsterHunterWilds;

    public DamageWidgetInitializer(IOverlay overlay)
    {
        _overlay = overlay;
    }

    public Task LoadAsync(IContext context)
    {
        if (context is MHWildsContext)
        {
            _logger.Warning($"Damage Widget has been temporarily disabled due to Monster Hunter Wilds v1.40.0.0 update. Full support for the newest update is still under development.");
            return Task.CompletedTask;
        }

        DamageMeterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            it => it.DamageMeterWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        var viewModel = new MeterViewModelV2(config);

        _view = _overlay.Register(viewModel);

        _handler = new DamageMeterControllerV2(
            context: context,
            viewModel: viewModel,
            widgetContext: (WidgetContext)_view.DataContext,
            config: config
        );

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}