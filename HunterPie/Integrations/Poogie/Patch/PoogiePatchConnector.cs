using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Patch.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Patch;

internal class PoogiePatchConnector(IPoogieClientAsync client)
{
    private readonly IPoogieClientAsync _client = client;

    private const string PATCH_ENDPOINT = "/v1/patch/notes";

    public async Task<PoogieResult<PatchResponse[]>> FindAll() =>
        await _client.GetAsync<PatchResponse[]>(PATCH_ENDPOINT);
}