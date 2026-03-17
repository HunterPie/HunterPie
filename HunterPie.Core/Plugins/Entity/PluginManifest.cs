using Newtonsoft.Json;
using System;

namespace HunterPie.Core.Plugins.Entity;

public record PluginManifest(
    [field: JsonProperty("name")] string Name,
    [field: JsonProperty("author")] string Author,
    [field: JsonProperty("version")] Version Version,
    [field: JsonProperty("description")] string Description
);
