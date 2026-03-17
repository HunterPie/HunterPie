using HunterPie.Core.Game;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.Entity;
using HunterPie.DI;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Services;

internal class PluginLoader(
    PluginProvider provider,
    IDependencyRegistry registry
)
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly ConcurrentDictionary<Plugin, IPlugin> _instances = new();


    public async Task InitializeAsync(IContext context)
    {
        IReadOnlyList<Plugin> plugins = await provider.GetAsync();

        _logger.Info($"Loading {plugins.Count} plugins...");

        foreach (Plugin plugin in plugins)
        {
            var instance = registry.Get(plugin.Type) as IPlugin;

            if (instance is not { })
                continue;

            if (!_instances.TryAdd(plugin, instance))
                _logger.Warning($"Failed to add plugin instance: {plugin.Manifest.Name}");

            await instance.InitializeAsync(context);
        }
    }

    public void Unload()
    {
        foreach (IPlugin instance in _instances.Values)
            instance.Dispose();

        _instances.Clear();
    }
}
