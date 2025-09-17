using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.ViewModels;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Damage;
using HunterPie.UI.Overlay.Widgets.Damage.Controllers;
using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class DamageWidgetInitializer : IWidgetInitializer
{
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
        DamageMeterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            it => it.DamageMeterWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        MeterViewModel viewModel = context switch
        {
            MHWildsContext or MHWContext => new MeterViewModelV2(config),
            _ => new MeterViewModel(config)
        };

        _view = _overlay.Register(viewModel);

        _handler = context switch
        {
            MHWildsContext or MHWContext => new DamageMeterControllerV2(
                context: context,
                viewModel: viewModel,
                widgetContext: (WidgetContext)_view.DataContext,
                config: config
            ),
            _ => new DamageMeterWidgetContextHandler(
                context: context,
                viewModel: viewModel,
                localizationRepository: DependencyContainer.Get<ILocalizationRepository>()
            )
        };

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
    }
}