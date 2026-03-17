using HunterPie.Core.Game;
using System;
using System.Threading.Tasks;

namespace HunterPie.Core.Plugins.Entity;

/// <summary>
/// Represents a HunterPie plugin, which is a modular component that can be loaded and unloaded at runtime to extend the functionality of the HunterPie application. 
/// Plugins can interact with the game context to provide features such as custom overlays, event handling, or additional tools for players. 
/// Each plugin must implement the IPlugin interface, which requires an Initialize method for setting up the plugin and a Dispose method for cleaning up resources when the plugin is unloaded.
/// </summary>
public interface IPlugin : IDisposable
{
    /// <summary>
    /// Initializes the plugin with the given game context. This method is called when the plugin is loaded, allowing it to set up any necessary resources or event handlers based on the current game state
    /// </summary>
    /// <param name="context">The game context that provides access to game state and events.</param>
    public Task InitializeAsync(IContext context);
}