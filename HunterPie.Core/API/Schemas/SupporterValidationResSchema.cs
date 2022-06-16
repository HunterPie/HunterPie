using Newtonsoft.Json;

namespace HunterPie.Core.API.Schemas
{
    public class SupporterValidationResSchema
    {
        [JsonProperty("is_valid")]
        public bool IsValid { get; set; }
    }
}
