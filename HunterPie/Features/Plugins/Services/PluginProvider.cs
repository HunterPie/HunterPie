using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.DI;
using HunterPie.Core.Plugins.Entity;
using HunterPie.DI;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace HunterPie.Features.Plugins.Services;

internal class PluginProvider
{
    private const bool CanUnloadAssembly = true;

    private record struct PluginContext(
        Plugin Plugin,
        AssemblyLoadContext Context
    );

    private readonly Dictionary<string, PluginContext> _contexts = new();

    private readonly ILogger _logger = LoggerFactory.Create();


    public async Task LoadAsync(IDependencyRegistry registry)
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

            var context = new AssemblyLoadContext(plugin, CanUnloadAssembly);

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

            var modules = context.Assemblies.SelectMany(a => a.GetTypes())
                .Where(t => typeof(IPluginModule).IsAssignableFrom(t) && !t.IsAbstract)
                .ToImmutableArray();

            foreach (Type moduleType in modules)
            {
                var module = Activator.CreateInstance(moduleType) as IPluginModule;

                if (module is not { })
                {
                    _logger.Warning($"Failed to create instance of module '{moduleType.FullName}' for plugin '{manifest.Name}'");
                    continue;
                }

                module.Register(registry);
            }

            Type? pluginType = context.Assemblies.SelectMany(it => it.GetTypes())
                .FirstOrDefault(it => typeof(IPlugin).IsAssignableFrom(it) && !it.IsAbstract);

            if (pluginType is null)
            {
                _logger.Warning($"No plugin type found for plugin '{manifest.Name}'");
                continue;
            }

            _contexts[plugin] = new PluginContext(
                Plugin: new Plugin(manifest, pluginType),
                Context: context
            );
        }
    }

    public async Task<IReadOnlyList<Plugin>> GetAsync()
    {
        return _contexts.Values.Select(c => c.Plugin)
            .ToImmutableArray();
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
