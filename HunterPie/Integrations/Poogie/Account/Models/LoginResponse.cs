using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LoginResponse([JsonProperty("token")] string Token);
