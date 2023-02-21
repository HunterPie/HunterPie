using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record MyUserAccountResponse(
    [JsonProperty("username")] string Username,
    [JsonProperty("email")] string Email,
    [JsonProperty("avatar_url")] string AvatarUrl,
    [JsonProperty("experience")] int Experience,
    [JsonProperty("rating")] int Rating,
    [JsonProperty("badges")] List<object> Badges,
    [JsonProperty("is_supporter")] bool IsSupporter
);