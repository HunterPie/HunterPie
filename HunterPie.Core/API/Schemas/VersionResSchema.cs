using Newtonsoft.Json;

namespace HunterPie.Core.API.Schemas
{
    public class VersionResSchema
    {
        [JsonProperty("latest_version")]
        public string LatestVersion { get; set; }
    }
}
