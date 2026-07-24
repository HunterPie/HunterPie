using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.Entity;
using HunterPie.DI;
using HunterPie.Features.Plugins.Entity;
using HunterPie.Features.Plugins.Repository;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Services;

internal class PluginLoader(
    IPluginRepository repository,
    IScopedDependencyRegistry registry
)
{
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly ConcurrentDictionary<Plugin, IPlugin> _instances = new();

    public async Task LoadAsync()
    {
        IReadOnlyList<PluginContext> pluginsContexts = repository.FindAll();

        _logger.Info($"Loading {pluginsContexts.Count} plugins...");

        foreach (PluginContext context in pluginsContexts)
        {
            Plugin plugin = context.Plugin;

            IScopedDependencyRegistry scope = registry.NewScope();

            scope.WithSingle(plugin.Configuration.GetType(), (_) => plugin.Configuration);

            context.Module.Register(scope);

            var instance = scope.Get(plugin.Type) as IPlugin;

            if (instance is not { })
                continue;

            if (!_instances.TryAdd(plugin, instance))
                _logger.Warning($"Failed to add plugin instance: {plugin.Manifest.Name}");

            try
            {
                await instance.InitializeAsync();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to initialize plugin '{plugin.Manifest.Name}': {ex}");
            }
        }
    }

    public void Unload()
    {
        foreach (IPlugin instance in _instances.Values)
            instance.Dispose();

        _instances.Clear();
    }
}