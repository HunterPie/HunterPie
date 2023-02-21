using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LoginRequest(
    [JsonProperty("email")] string Email,
    [JsonProperty("password")] string Password
);
