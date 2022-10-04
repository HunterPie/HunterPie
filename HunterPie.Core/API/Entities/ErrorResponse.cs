using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class ErrorResponse
{
    [JsonProperty("code")]
    public ErrorCode Code { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }
}
