using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;

public class LoginResponse
{
    [JsonProperty("token")]
    public string Token { get; set; }
}
