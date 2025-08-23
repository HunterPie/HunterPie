using HunterPie.Features.Theme.Entity;
using Newtonsoft.Json;

namespace HunterPie.Features.Theme.Repository.Model;

internal record ThemeManifestModel(
    [field: JsonProperty("id")] string Id,
    [field: JsonProperty("name")] string Name,
    [field: JsonProperty("description")] string Description,
    [field: JsonProperty("version")] string Version,
    [field: JsonProperty("author")] string Author,
    [field: JsonProperty("tags")] string[] Tags
)
{
    internal ThemeManifest ToEntity()
    {
        return new ThemeManifest(
            Id: Id,
            Name: Name,
            Description: Description,
            Version: Version,
            Author: Author,
            Tags: Tags
        );
    }
}