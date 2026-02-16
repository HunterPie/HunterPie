using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Scan.Service;
using HunterPie.DI;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class DebugWidgetInitializer(
    IFeatureFlagRepository featureFlagRepository,
    IOverlay overlay) : IInitializer
{
    private readonly IFeatureFlagRepository _featureFlagRepository = featureFlagRepository;
    private readonly IOverlay _overlay = overlay;

    public Task Init()
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(GameProcessType.MonsterHunterRise);

        if (_featureFlagRepository.IsEnabled(FeatureFlags.FEATURE_METRICS_WIDGET))
            _overlay.Register(
                new TelemetricsViewModel(
                    config: config.DebugWidget,
                    scanService: DependencyContainer.Get<IScanService>()
                )
            );

        return Task.CompletedTask;
    }
}