using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Common.Models;

internal record PoogieError(
    [JsonProperty("code", Required = Required.Always)] PoogieErrorCode Code,
    [JsonProperty("error", Required = Required.Always)] string Error
);
