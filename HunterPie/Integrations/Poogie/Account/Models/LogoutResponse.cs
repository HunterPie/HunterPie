using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;

internal record LogoutResponse([JsonProperty("message")] string Message);
