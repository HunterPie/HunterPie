using HunterPie.Core.Analytics.Entity;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Strategies;

internal interface IAnalyticsStrategy<in TIn> where TIn : IAnalyticsEvent
{
    bool CanHandle(Type type);
    Task SendAsync(TIn analyticsEvent);
}