using HunterPie.Features.Statistics.Models;
using Newtonsoft.Json;
using System.Linq;

namespace HunterPie.Integrations.Poogie.Statistics.Models;

internal record PoogieAbnormalityStatisticsModel(
    [JsonProperty("id")] string Id,
    [JsonProperty("activations")] PoogieTimeFrameModel[] Activations
)
{
    public AbnormalityModel ToEntity() => new AbnormalityModel(
        Id: Id,
        Activations: Activations.Select(it => it.ToEntity()).ToArray()
    );

    public static PoogieAbnormalityStatisticsModel From(AbnormalityModel model) =>
        new PoogieAbnormalityStatisticsModel(
            Id: model.Id,
            Activations: model.Activations.Select(PoogieTimeFrameModel.From).ToArray()
        );
}
