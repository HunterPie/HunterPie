using HunterPie.Core.Plugins.Configuration;
using System;

namespace HunterPie.Core.Plugins.Entity;

public record Plugin(
    PluginManifest Manifest,
    PluginConfiguration Configuration,
    Type Type
);
