using System;

namespace HunterPie.Features.Analytics.Services;

internal class ClientMetrics
{
    public readonly DateTime StartedAt;

    public ClientMetrics()
    {
        StartedAt = DateTime.UtcNow;
    }
}