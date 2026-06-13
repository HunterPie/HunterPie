using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Plugins.Entity;

/// <summary>
/// Represents a HunterPie plugin, which is a modular component that can be loaded and unloaded at runtime to extend the functionality of HunterPie.
/// Plugins can interact with the game context to provide features such as custom overlays, event handling, or additional tools for players. 
/// Each plugin must implement the IPlugin interface, which requires an InitializeAsync method for setting up the plugin and a Dispose method for cleaning up resources when the plugin is unloaded.
/// </summary>
public interface IPlugin : IDisposable
{
    /// <summary>
    /// Initializes the plugin, this is the real entrypoint of your plugin
    /// </summary>
    /// <returns>Awaitable Task</returns>
    public Task InitializeAsync();
}