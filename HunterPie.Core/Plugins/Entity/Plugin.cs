using System;

namespace HunterPie.Core.Plugins.Entity;

public record Plugin(
    PluginManifest Manifest,
    Type Type
);
