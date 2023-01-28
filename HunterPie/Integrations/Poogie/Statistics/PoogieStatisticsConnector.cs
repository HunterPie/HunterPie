using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Statistics;
internal class PoogieStatisticsConnector
{
    private readonly PoogieConnector _connector = new();

    private const string STATISTICS_ENDPOINT = "/v1/hunt";

    public async Task<PoogieResult<PoogieQuestStatisticsModel>> Upload(PoogieQuestStatisticsModel model) =>
        await _connector.Post<PoogieQuestStatisticsModel, PoogieQuestStatisticsModel>($"{STATISTICS_ENDPOINT}/upload", model);
}
