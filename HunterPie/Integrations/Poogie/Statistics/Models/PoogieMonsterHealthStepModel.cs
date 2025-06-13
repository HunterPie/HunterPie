using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieMonsterHealthStepModel(
    [property: JsonProperty("percentage")] double Percentage,
    [property: JsonProperty("time")] DateTime Time
)
{
    public MonsterHealthStepModel ToEntity()
    {
        return new MonsterHealthStepModel(
            Percentage: Percentage,
            Time: Time
        );
    }

    public static PoogieMonsterHealthStepModel From(MonsterHealthStepModel model) =>
        new(
            Percentage: model.Percentage,
            Time: model.Time
        );
}