using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Interfaces;

namespace HunterPie.Core.Plugins.Configuration;

/// <summary>
/// PluginConfiguration is the base class for all plugin configurations.
/// </summary>
public abstract class PluginConfiguration : IVersionedConfig
{
    /// <summary>
    /// Version is used to control settings migrations. 
    /// Whenever a breaking change is made to the plugin configuration, this version should be incremented, and a migration should be added to the plugin's configuration migrator.
    /// </summary>
    public abstract int Version { get; }

    /// <summary>
    /// IsEnabled controls whether the plugin is enabled or not. This is used by the plugin manager to determine whether to load the plugin or not.
    /// </summary>
    public Observable<bool> IsEnabled { get; set; } = true;

}
