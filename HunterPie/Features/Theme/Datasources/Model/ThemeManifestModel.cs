using HunterPie.Features.Theme.Entity;
using Newtonsoft.Json;

namespace HunterPie.Features.Theme.Datasources.Model;

internal record ThemeManifestModel(
    [field: JsonProperty("name")] string Name,
    [field: JsonProperty("description")] string Description,
    [field: JsonProperty("author")] string Author,
    [field: JsonProperty("isVerified")] bool IsVerified,
    [field: JsonProperty("assetUrls")] string[] AssetUrls
)
{
    internal ThemeManifest ToEntity()
    {
        return new ThemeManifest(
            Name: Name,
            Description: Description,
            Author: Author,
            IsVerified: IsVerified,
            AssetUrls: AssetUrls
        );
    }
}