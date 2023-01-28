using HunterPie.Core.Game.Enums;
using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogiePlayerStatisticsModel(
    [JsonProperty("name")] string Name,
    [JsonProperty("weapon")] Weapon Weapon,
    [JsonProperty("damages")] PoogiePlayerDamageStatisticsModel[] Damages,
    [JsonProperty("abnormalities")] PoogieAbnormalityStatisticsModel[] Abnormalities,
    [JsonProperty("is_hunterpie_user")] bool IsHunterPieUser
)
{
    public PartyMemberModel ToEntity() => new PartyMemberModel(
        Name: Name,
        Weapon: Weapon,
        Damages: Damages.Select(it => it.ToEntity()).ToArray(),
        Abnormalities: Abnormalities.Select(it => it.ToEntity()).ToArray(),
        IsHunterPieUser: IsHunterPieUser
    );

    public static PoogiePlayerStatisticsModel From(PartyMemberModel model) =>
        new PoogiePlayerStatisticsModel(
            Name: model.Name,
            Weapon: model.Weapon,
            Damages: model.Damages.Select(PoogiePlayerDamageStatisticsModel.From).ToArray(),
            Abnormalities: model.Abnormalities.Select(PoogieAbnormalityStatisticsModel.From).ToArray(),
            IsHunterPieUser: model.IsHunterPieUser
        );
}