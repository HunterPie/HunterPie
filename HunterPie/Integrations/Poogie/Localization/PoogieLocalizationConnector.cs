using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Localization.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Localization;

internal class PoogieLocalizationConnector(IPoogieClientAsync client)
{
    private readonly IPoogieClientAsync _client = client;

    private const string LOCALIZATION_ENDPOINT = "/v1/localization";

    public async Task<PoogieResult<LocalizationResponse>> GetChecksumsAsync() =>
        await _client.GetAsync<LocalizationResponse>($"{LOCALIZATION_ENDPOINT}/checksum");
}