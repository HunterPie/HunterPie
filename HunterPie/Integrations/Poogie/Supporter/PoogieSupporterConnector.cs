using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Supporter.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Supporter;
internal class PoogieSupporterConnector
{
    private readonly PoogieConnector _connector = new();

    private const string SUPPORTER_ENDPOINT = "/v1/supporter";

    public async Task<PoogieResult<SupporterValidationResponse>> IsValidSupporter() =>
        await _connector.Get<SupporterValidationResponse>($"{SUPPORTER_ENDPOINT}/verify");
}
