using HunterPie.Core.Analytics.Entity;
using HunterPie.Integrations.Poogie.Report;
using HunterPie.Integrations.Poogie.Report.Models;
using System;
using System.Threading.Tasks;

namespace HunterPie.Features.Analytics.Strategies;

internal class CrashEventStrategy : IAnalyticsStrategy<CrashPayload>
{
    private readonly PoogieReportConnector _connector;

    public CrashEventStrategy(PoogieReportConnector connector)
    {
        _connector = connector;
    }

    public bool CanHandle(Type type) => type == typeof(CrashPayload);

    public async Task SendAsync(CrashPayload analyticsEvent)
    {
        var request = new CrashReportRequest(
            Version: analyticsEvent.Version,
            GameBuild: analyticsEvent.GameBuild ?? "Unknown",
            Exception: analyticsEvent.Error,
            Stacktrace: analyticsEvent.Stacktrace,
            IsUiError: analyticsEvent.IsUiError,
            Context: new CrashReportContextRequest(
                RamTotal: analyticsEvent.Context.TotalSystemMemory,
                RamUsed: analyticsEvent.Context.AllocatedMemory,
                WindowsVersion: analyticsEvent.Context.WindowsVersion
            )
        );

        await _connector.CrashAsync(request);
    }
}