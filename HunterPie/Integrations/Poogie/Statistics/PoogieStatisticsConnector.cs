using HunterPie.Core.Domain.Cache;
using HunterPie.Core.Domain.Cache.Model;
using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Statistics.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Statistics;
#nullable enable
internal class PoogieStatisticsConnector
{
    private const string SUMMARIES_CACHE_KEY = "summaries";
    private const string UPLOAD_CACHE_KEY = "quest_upload_{0}";

    /// <summary>
    /// Common cache for all instances of this connector
    /// </summary>
    private static readonly IAsyncCache Cache = new InMemoryAsyncCache();

    private readonly PoogieConnector _connector = new();

    private const string STATISTICS_ENDPOINT = "/v1/hunt";

    public async Task<PoogieResult<PoogieQuestStatisticsModel>> Upload(PoogieQuestStatisticsModel model) =>
        await _connector.Post<PoogieQuestStatisticsModel, PoogieQuestStatisticsModel>($"{STATISTICS_ENDPOINT}/upload", model);

    public async Task<PoogieResult<List<PoogieQuestSummaryModel>>> GetUserQuestSummaries()
    {
        PoogieResult<List<PoogieQuestSummaryModel>>? cachedSummaries =
            await Cache.Get<PoogieResult<List<PoogieQuestSummaryModel>>>(SUMMARIES_CACHE_KEY);

        if (cachedSummaries != null)
            return cachedSummaries;

        PoogieResult<List<PoogieQuestSummaryModel>> result = await _connector.Get<List<PoogieQuestSummaryModel>>($"{STATISTICS_ENDPOINT}");

        if (result.Response is not null)
            await Cache.Set(
                key: SUMMARIES_CACHE_KEY,
                value: result,
                options: new CacheOptions(Ttl: TimeSpan.FromMinutes(1))
            );

        return result;
    }

    public async Task<PoogieResult<PoogieQuestStatisticsModel>> Get(string uploadId)
    {
        PoogieResult<PoogieQuestStatisticsModel>? cachedStatistics =
            await Cache.Get<PoogieResult<PoogieQuestStatisticsModel>>(string.Format(UPLOAD_CACHE_KEY, uploadId));

        if (cachedStatistics != null)
            return cachedStatistics;

        PoogieResult<PoogieQuestStatisticsModel> result = await _connector.Get<PoogieQuestStatisticsModel>($"{STATISTICS_ENDPOINT}/{uploadId}");

        if (result.Response is not null)
            await Cache.Set(
                key: string.Format(UPLOAD_CACHE_KEY, uploadId),
                value: result
            );

        return result;
    }
}
