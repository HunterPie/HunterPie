using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.DI;
using HunterPie.Core.Plugins.Entity;
using HunterPie.Features.Plugins.Entity;
using HunterPie.Features.Plugins.Services;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Repository;

internal class LocalPluginRepository : IPluginRepository
{
    private readonly ConcurrentDictionary<string, PluginContext> _contexts = new();

    private readonly ILogger _logger = LoggerFactory.Create();

    public async Task InitializeAsync()
    {
        if (!Directory.Exists(ClientInfo.PluginsPath))
            return;

        IEnumerable<string> plugins = Directory.EnumerateDirectories(ClientInfo.PluginsPath);

        foreach (string plugin in plugins)
        {
            string[] assemblies = Directory.EnumerateFiles(plugin, "*.dll")
                .ToArray();

            if (assemblies.Length == 0)
                continue;

            PluginManifest? manifest = await TryLoadManifestAsync(plugin);

            if (manifest is not { })
                continue;

            var context = new PluginLoadContext(plugin);

            foreach (string assembly in assemblies)
            {
                try
                {
                    context.LoadFromAssemblyPath(assembly);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to load assembly '{assembly}' for plugin '{manifest.Name}': {ex}");
                    context.Unload();
                    break;
                }
            }

            Type? moduleType = context.Assemblies.SelectMany(a => a.GetTypes())
                .SingleOrDefault(t => typeof(IPluginModule).IsAssignableFrom(t) && !t.IsAbstract);

            if (moduleType is not { })
            {
                _logger.Warning($"No module found for plugin '{manifest.Name}'");
                continue;
            }

            var module = Activator.CreateInstance(moduleType) as IPluginModule;

            if (module is not { })
            {
                _logger.Warning($"Failed to create instance of module '{moduleType.FullName}' for plugin '{manifest.Name}'");
                continue;
            }

            Type? pluginType = context.Assemblies.SelectMany(it => it.GetTypes())
                .FirstOrDefault(it => typeof(IPlugin).IsAssignableFrom(it) && !it.IsAbstract);

            if (pluginType is null)
            {
                _logger.Warning($"No plugin type found for plugin '{manifest.Name}'");
                continue;
            }

            _contexts[plugin] = new PluginContext(
                Plugin: new Plugin(
                    Manifest: manifest,
                    Configuration: module.Configuration,
                    Type: pluginType
                ),
                Module: module,
                Context: context
            );
        }
    }

    public IReadOnlyList<PluginContext> FindAll()
    {
        return _contexts.Values.ToImmutableArray();
    }

    private static async Task<PluginManifest?> TryLoadManifestAsync(string path)
    {
        string manifestPath = Path.Combine(path, "plugin.manifest.json");

        if (!File.Exists(manifestPath))
            return null;

        string manifestContent = await File.ReadAllTextAsync(manifestPath);

        return JsonProvider.Deserializer<PluginManifest>(manifestContent);
    }
}