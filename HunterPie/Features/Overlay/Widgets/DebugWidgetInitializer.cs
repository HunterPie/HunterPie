using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Scan.Service;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Service;
using HunterPie.UI.Overlay.Views;
using HunterPie.UI.Overlay.Widgets.Metrics.ViewModel;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay.Widgets;

internal class DebugWidgetInitializer(
    IFeatureFlagRepository featureFlagRepository,
    IOverlay overlay,
    OverlayConfig config,
    IScanService scanService
) : IWidgetInitializer
{
    public GameProcessType SupportedGames => GameProcessType.All;

    private WidgetView? _view;

    public Task LoadAsync()
    {
        if (!featureFlagRepository.IsEnabled(FeatureFlags.FEATURE_METRICS_WIDGET))
            return Task.CompletedTask;

        _view = overlay.Register(
            new TelemetricsViewModel(
                config: config.DebugWidget,
                scanService: scanService
            )
        );

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        overlay.Unregister(_view);
    }
}