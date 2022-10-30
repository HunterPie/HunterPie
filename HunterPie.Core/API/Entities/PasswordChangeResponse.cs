using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class PasswordChangeResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}
