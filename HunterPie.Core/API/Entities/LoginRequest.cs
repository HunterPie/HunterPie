using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities
{
    public class LoginRequest
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
