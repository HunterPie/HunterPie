using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;

public class PasswordResetRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }
}
