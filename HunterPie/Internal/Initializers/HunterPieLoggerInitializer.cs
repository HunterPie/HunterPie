using HunterPie.Core.Observability.Logging;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Logging.Services;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class HunterPieLoggerInitializer(
    HunterPieLogWriter writer
) : IInitializer
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public Task Init()
    {
        LoggerFactory.Add(writer);

        _logger.Info("Initialized HunterPie logger");

        return Task.CompletedTask;
    }
}