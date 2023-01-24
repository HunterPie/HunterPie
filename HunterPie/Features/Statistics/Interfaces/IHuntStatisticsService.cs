using System;

namespace HunterPie.Features.Statistics.Interfaces;

internal interface IHuntStatisticsService<out T> : IDisposable
{
    public T Export();
}
