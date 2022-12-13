using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record PasswordChangeResponse([JsonProperty("success")] bool Success);
