using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Report.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Report;
internal class PoogieReportConnector
{

    private readonly PoogieConnector _connector = new();

    private const string REPORT_ENDPOINT = "/v1/report";

    public async Task Crash(CrashReportRequest request) =>
        await _connector.Post<CrashReportRequest, Nothing>($"{REPORT_ENDPOINT}/crash", request);
}
