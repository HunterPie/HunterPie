using HunterPie.Features.Statistics.Models;
using HunterPie.Integrations.Poogie.Common.Models;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieQuestStatisticsModel(
    [JsonProperty("game_type")] GameType GameType,
    [JsonProperty("started_at")] DateTime StartedAt,
    [JsonProperty("finished_at")] DateTime FinishedAt,
    [JsonProperty("player")] PoogiePlayerStatisticsModel[] Players,
    [JsonProperty("monsters")] PoogieMonsterStatisticsModel[] Monsters,
    [JsonProperty("hash")] string Hash
)
{
    public HuntStatisticsModel ToEntity() =>
        new HuntStatisticsModel(
            Game: GameType.ToEntity(),
            StartedAt: StartedAt,
            FinishedAt: FinishedAt,
            Players: Players.Select(it => it.ToEntity()).ToList(),
            Monsters: Monsters.Select(it => it.ToEntity()).ToList(),
            Hash: Hash
        );

    public static PoogieQuestStatisticsModel From(HuntStatisticsModel model) =>
        new PoogieQuestStatisticsModel(
            GameType: model.Game.ToApiModel(),
            StartedAt: model.StartedAt,
            FinishedAt: model.FinishedAt,
            Players: model.Players.Select(PoogiePlayerStatisticsModel.From).ToArray(),
            Monsters: model.Monsters.Select(PoogieMonsterStatisticsModel.From).ToArray(),
            Hash: model.Hash
        );
}