using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.Platforms.Common.Logging;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NativeLoggerInitializer : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly INativeLogWriter _logWriter;

    public NativeLoggerInitializer(INativeLogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    public Task Init()
    {
        if (!FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_NATIVE_LOGGER))
            return Task.CompletedTask;

        LoggerFactory.Add(_logWriter);

        _logger.Info("Hello world! HunterPie native logger has been initialized!");

        return Task.CompletedTask;
    }
}