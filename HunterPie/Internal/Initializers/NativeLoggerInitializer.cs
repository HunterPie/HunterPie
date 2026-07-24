using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Platforms.Common.Logging;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NativeLoggerInitializer(
    INativeLogWriter logWriter,
    IFeatureFlagRepository featureFlagRepository
) : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task Init()
    {
        if (!featureFlagRepository.IsEnabled(FeatureFlags.FEATURE_NATIVE_LOGGER))
            return Task.CompletedTask;

        logWriter.CreateTerminal();
        LoggerFactory.Add(logWriter);

        _logger.Info("Hello world! HunterPie native logger has been initialized!");

        return Task.CompletedTask;
    }
}