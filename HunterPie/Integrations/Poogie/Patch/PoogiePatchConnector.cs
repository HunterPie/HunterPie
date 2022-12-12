using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Patch.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Patch;
internal class PoogiePatchConnector
{
    private readonly PoogieConnector _connector = new();

    private const string PATCH_ENDPOINT = "/v1/patch/notes";

    public async Task<PoogieResult<PatchResponse[]>> FindAll() =>
        await _connector.Get<PatchResponse[]>(PATCH_ENDPOINT);
}
