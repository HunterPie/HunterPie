using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Integrations.Datasources.Common.Monster;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Monster;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class MonsterWidgetInitializer(
    IContext context,
    IOverlay overlay,
    OverlayConfig config,
    DistanceFunc distanceFunc
) : IWidgetInitializer
{
    private readonly IOverlay _overlay = overlay;

    private WeightedTargetDetectionService? _targetDetectionService;
    private MonsterWidgetContextHandler? _handler;
    private WidgetView? _view;

    public GameProcessType SupportedGames =>
        GameProcessType.MonsterHunterRise |
        GameProcessType.MonsterHunterWorld |
        GameProcessType.MonsterHunterWilds;

    public Task LoadAsync()
    {
        if (!config.BossesWidget.Initialize)
            return Task.CompletedTask;

        var viewModel = new MonstersViewModel(
            settings: config.BossesWidget
        );

        _targetDetectionService = new WeightedTargetDetectionService(
            context: context,
            distanceFunc: distanceFunc
        );
        _targetDetectionService.Initialize();

        _handler = new MonsterWidgetContextHandler(
            context: context,
            targetDetectionService: _targetDetectionService,
            viewModel: viewModel,
            config: config.BossesWidget
        );

        _view = _overlay.Register(viewModel);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _overlay.Unregister(_view);
        _handler?.UnhookEvents();
        _handler = null;
        _targetDetectionService?.Dispose();
        _targetDetectionService = null;
    }
}