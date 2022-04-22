using Newtonsoft.Json;

namespace HunterPie.Core.API.Schemas
{
    public class SessionResSchema
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }
    }
}
