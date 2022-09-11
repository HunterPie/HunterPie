using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities
{
    public class VersionResponse
    {
        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; }
    }
}
