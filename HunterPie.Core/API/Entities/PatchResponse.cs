using Newtonsoft.Json;

namespace HunterPie.Core.API.Entities;
public class PatchResponse
{
    [JsonProperty("title")]
    public string Title { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("banner")]
    public string Banner { get; set; }

    [JsonProperty("link")]
    public string Link { get; set; }
}
