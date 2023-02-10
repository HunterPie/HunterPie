using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record RegisterResponse(
    [JsonProperty("username")] string Username,
    [JsonProperty("email")] string Email,
    [JsonProperty("avatar_url")] string AvatarUrl,
    [JsonProperty("experience")] int Experience,
    [JsonProperty("rating")] int Rating,
    [JsonProperty("is_supporter")] bool IsSupporter
);
