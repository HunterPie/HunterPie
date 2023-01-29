using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieMonsterStatisticsModel(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("max_health")] float MaxHealth,
    [property: JsonProperty("crown")] Crown Crown,
    [property: JsonProperty("enrage")] PoogieMonsterStatusStatisticsModel Enrage,
    [property: JsonProperty("hunt_started_at")] DateTime? HuntStartedAt,
    [property: JsonProperty("hunt_finished_at")] DateTime? HuntFinishedAt
)
{
    public MonsterModel ToEntity() => new MonsterModel(
        Id: Id,
        MaxHealth: MaxHealth,
        Crown: Crown,
        Enrage: Enrage.ToEntity(),
        HuntStartedAt: HuntStartedAt,
        HuntFinishedAt: HuntFinishedAt
    );

    public static PoogieMonsterStatisticsModel From(MonsterModel model) =>
        new PoogieMonsterStatisticsModel(
            Id: model.Id,
            MaxHealth: model.MaxHealth,
            Crown: model.Crown,
            Enrage: PoogieMonsterStatusStatisticsModel.From(model.Enrage),
            HuntStartedAt: model.HuntStartedAt,
            HuntFinishedAt: model.HuntFinishedAt
        );
}