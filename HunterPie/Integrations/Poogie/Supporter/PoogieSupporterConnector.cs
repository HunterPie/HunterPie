using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Supporter.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Supporter;

internal class PoogieSupporterConnector(IPoogieClientAsync client)
{
    private readonly IPoogieClientAsync _client = client;

    private const string SUPPORTER_ENDPOINT = "/v1/supporter";

    public async Task<PoogieResult<SupporterValidationResponse>> IsValidSupporterAsync() =>
        await _client.GetAsync<SupporterValidationResponse>($"{SUPPORTER_ENDPOINT}/verify");
}