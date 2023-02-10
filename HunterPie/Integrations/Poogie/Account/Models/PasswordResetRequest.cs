using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Account.Models;
internal record PasswordResetRequest([JsonProperty("email")] string Email);
