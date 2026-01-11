using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Report.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Report;

internal class PoogieReportConnector(IPoogieClientAsync client)
{
    private readonly IPoogieClientAsync _client = client;

    private const string REPORT_ENDPOINT = "/v1/report";

    public async Task CrashAsync(CrashReportRequest request) =>
        await _client.PostAsync<CrashReportRequest, Nothing>($"{REPORT_ENDPOINT}/crash", request);
}