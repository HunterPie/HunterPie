using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Localization.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Localization;
internal class PoogieLocalizationConnector
{
    private readonly PoogieConnector _connector = new();

    private const string LOCALIZATION_ENDPOINT = "/v1/localization";

    public async Task<PoogieResult<LocalizationResponse>> GetChecksums() =>
        await _connector.Get<LocalizationResponse>($"{LOCALIZATION_ENDPOINT}/checksum");
}
