using HunterPie.Core.Analytics.Entity;
using HunterPie.Features.Analytics.Entity;
using HunterPie.Features.Analytics.Services;
using HunterPie.Integrations.Poogie.Report;
using HunterPie.Integrations.Poogie.Report.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Strategies;

internal class CrashEventStrategy(
    ClientMetrics clientMetrics,
    PoogieReportConnector connector
    ) : IAnalyticsStrategy
{
    private readonly ClientMetrics _clientMetrics = clientMetrics;
    private readonly PoogieReportConnector _connector = connector;

    public bool CanHandle(Type type) => type == typeof(CrashPayload);

    public async Task SendAsync(IAnalyticsEvent analyticsEvent)
    {
        if (analyticsEvent is not CrashPayload payload)
            return;

        DateTime now = DateTime.UtcNow;

        var request = new CrashReportRequest(
            Version: payload.Version,
            GameBuild: payload.GameBuild ?? "Unknown",
            Exception: payload.Error,
            Stacktrace: payload.Stacktrace,
            IsUiError: payload.IsUiError,
            Context: new CrashReportContextRequest(
                RamTotal: payload.Context.TotalSystemMemory,
                RamUsed: payload.Context.AllocatedMemory,
                WindowsVersion: payload.Context.WindowsVersion,
                SessionTime: (now - _clientMetrics.StartedAt).ToString()
            )
        );

        await _connector.CrashAsync(request);
    }
}