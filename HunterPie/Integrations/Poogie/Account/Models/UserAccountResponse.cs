using Newtonsoft.Json;
using System.Collections.Generic;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record UserAccountResponse(
    [JsonProperty("username")] string Username,
    [JsonProperty("avatar_url")] string AvatarUrl,
    [JsonProperty("rating")] string Rating,
    [JsonProperty("badges")] List<object> Badges,
    [JsonProperty("is_supporter")] bool IsSupporter
);