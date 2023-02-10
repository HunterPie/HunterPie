using Newtonsoft.Json;

namespace HunterPie.Integrations.Poogie.Patch.Models;

public record PatchResponse(
    [JsonProperty("title")] string Title,
    [JsonProperty("description")] string Description,
    [JsonProperty("banner")] string Banner,
    [JsonProperty("link")] string Link
);