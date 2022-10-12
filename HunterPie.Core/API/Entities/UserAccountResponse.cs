using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Core.API.Entities;

public class UserAccountResponse
{
    [JsonProperty("username")]
    public string Username { get; set; }

    [JsonProperty("avatar_url")]
    public string AvatarUrl { get; set; }

    [JsonProperty("rating")]
    public int Rating { get; set; }

    [JsonProperty("badges")]
    public List<object> Badges { get; set; }

    [JsonProperty("is_supporter")]
    public bool IsSupporter { get; set; }
}
