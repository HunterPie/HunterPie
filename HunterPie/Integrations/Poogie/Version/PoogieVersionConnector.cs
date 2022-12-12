using HunterPie.Core.Networking.Http;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Version;
internal class PoogieVersionConnector
{
    private readonly PoogieConnector _connector = new();

    private const string VERSION_ENDPOINT = "/v1/version";

    public async Task<PoogieResult<VersionResponse>> Latest() =>
        await _connector.Get<VersionResponse>(VERSION_ENDPOINT);

    public async Task<HttpClientResponse> Download(string version) =>
        await _connector.Download($"{VERSION_ENDPOINT}/{version}");
}
