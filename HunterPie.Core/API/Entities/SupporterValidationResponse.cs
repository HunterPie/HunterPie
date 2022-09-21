using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;

public class SupporterValidationResponse
{
    [JsonProperty("is_valid")]
    public bool IsValid { get; set; }
}
