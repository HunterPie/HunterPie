using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieMonsterStatisticsModel(
    [property: JsonProperty("id")] int Id,
    [property: JsonProperty("max_health")] float MaxHealth,
    [property: JsonProperty("crown")] Crown Crown,
    [property: JsonProperty("enrage")] PoogieMonsterStatusStatisticsModel Enrage,
    [property: JsonProperty("hunt_started_at")] DateTime? HuntStartedAt,
    [property: JsonProperty("hunt_finished_at")] DateTime? HuntFinishedAt,
    [property: JsonProperty("hunt_type")] MonsterHuntType? HuntType,
    [property: JsonProperty("variant")] int? Variant,
    [property: JsonProperty("health_steps")] List<PoogieMonsterHealthStepModel>? HealthSteps
)
{
    public MonsterModel ToEntity() => new(
        Id: Id,
        MaxHealth: MaxHealth,
        Crown: Crown,
        Enrage: Enrage.ToEntity(),
        HuntStartedAt: HuntStartedAt,
        HuntFinishedAt: HuntFinishedAt,
        HuntType: HuntType,
        Variant: Variant as VariantType? ?? VariantType.Normal,
        HealthSteps: HealthSteps?.Select(it => it.ToEntity())
            .ToList() ?? new List<MonsterHealthStepModel>()
    );

    public static PoogieMonsterStatisticsModel From(MonsterModel model) =>
        new(
            Id: model.Id,
            MaxHealth: model.MaxHealth,
            Crown: model.Crown,
            Enrage: PoogieMonsterStatusStatisticsModel.From(model.Enrage),
            HuntStartedAt: model.HuntStartedAt,
            HuntFinishedAt: model.HuntFinishedAt,
            HuntType: model.HuntType,
            Variant: model.Variant,
            HealthSteps: model.HealthSteps.Select(PoogieMonsterHealthStepModel.From).ToList()
        );
}