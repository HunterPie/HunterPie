using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class RegisterRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }
}
