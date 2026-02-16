using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.Common.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Monster;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Numerics;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class MonsterWidgetInitializer(IOverlay overlay) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private WeightedTargetDetectionService? _targetDetectionService;
    private IContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise |
        GameProcessType.MonsterHunterWorld |
        GameProcessType.MonsterHunterWilds;

    public Task LoadAsync(IContext context)
    {
        MonsterWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
            game: context.Process.Type,
            (config) => config.BossesWidget
        );

        if (!config.Initialize)
            return Task.CompletedTask;

        var viewModel = new MonstersViewModel(
            settings: config
        );

        DistanceFunc distanceFunc = context switch
        {
            MHWContext => static (Vector3 playerPosition, Vector3 monsterPosition) => Vector3.Distance(playerPosition, monsterPosition) / 100.0f,
            _ => Vector3.Distance
        };

        _targetDetectionService = new WeightedTargetDetectionService(
            context: context,
            distanceFunc: distanceFunc
        );
        _targetDetectionService.Initialize();

        _handler = new MonsterWidgetContextHandler(
            context: context,
            targetDetectionService: _targetDetectionService,
            viewModel: viewModel,
            config: config
        );

        _view = _overlay.Register(viewModel);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
        _targetDetectionService?.Dispose();
        _targetDetectionService = null;
    }
}