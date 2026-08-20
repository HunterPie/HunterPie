using HunterPie.Core.Networking.Http.Models;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Version.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Version;

internal class PoogieVersionConnector(IPoogieClientAsync client)
{
    private const string VERSION_ENDPOINT = "/v1/version";

    public async Task<PoogieResult<VersionResponse>> Latest() =>
        await client.GetAsync<VersionResponse>(VERSION_ENDPOINT);

    public async Task<Response.Success> Download(string version) =>
        await client.DownloadAsync($"{VERSION_ENDPOINT}/{version}");
}