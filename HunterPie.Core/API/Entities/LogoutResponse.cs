using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class LogoutResponse
{
    [JsonProperty("message")]
    public string Message { get; set; }
}
