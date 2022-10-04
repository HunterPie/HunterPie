using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class RegisterResponse
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("experience")]
    public int Experience { get; set; }

    [JsonProperty("rating")]
    public int Rating { get; set; }

    [JsonProperty("is_supporter")]
    public bool IsSupporter { get; set; }
}
