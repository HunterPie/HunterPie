using HunterPie.Core.Observability.Logging;
using System.Diagnostics;

namespace HunterPie.Features.Observability.Tracing;

internal sealed class UITracer : TraceListener
{
    private readonly ILogger _logger = LoggerFactory.Create();

    public override void Write(string? message)
    {

    }

    public override void WriteLine(string? message)
    {
        if (message is not { })
            return;

        _logger.Error(message);
    }
}