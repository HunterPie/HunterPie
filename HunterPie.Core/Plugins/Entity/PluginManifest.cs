using System;

namespace HunterPie.Core.Plugins.Entity;

public record PluginManifest(
    string Name,
    string Author,
    Version Version,
    string Description
);
