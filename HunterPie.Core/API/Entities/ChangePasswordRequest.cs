using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class ChangePasswordRequest
{
    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("code")]
    public string Code { get; set; }

    [JsonProperty("new_password")]
    public string NewPassword { get; set; }
}
