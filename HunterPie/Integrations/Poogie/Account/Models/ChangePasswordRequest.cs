using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record ChangePasswordRequest(
    [JsonProperty("email")] string Email,
    [JsonProperty("code")] string Code,
    [JsonProperty("new_password")] string NewPassword
);
