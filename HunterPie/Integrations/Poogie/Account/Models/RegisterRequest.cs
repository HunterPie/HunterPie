using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record RegisterRequest(
    [JsonProperty("email")] string Email,
    [JsonProperty("username")] string Username,
    [JsonProperty("password")] string Password
);