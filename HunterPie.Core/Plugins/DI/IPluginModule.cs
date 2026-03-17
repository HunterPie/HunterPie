using HunterPie.DI.Module;

namespace HunterPie.Core.Plugins.DI;

/// <summary>
/// Class responsible for registering plugin dependencies. 
/// Each plugin should have its own implementation of this interface, which will be used to register the plugin's services and dependencies in the DI container.
/// </summary>
public interface IPluginModule : IDependencyModule;
