using System;

namespace HunterPie.Features.Statistics.Models;

internal record TimeFrameModel(
    TimeSpan StartedAt,
    TimeSpan FinishedAt
)
{
    public static TimeFrameModel Start(DateTime start) => new TimeFrameModel(
        StartedAt: DateTime.UtcNow - start,
        FinishedAt: TimeSpan.MaxValue
    );

    public TimeFrameModel End(DateTime start) => this with { FinishedAt = DateTime.UtcNow - start };

    public bool IsRunning() => FinishedAt == TimeSpan.MaxValue;
}
