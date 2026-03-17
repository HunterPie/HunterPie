using HunterPie.Core.Client;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.Entity;
using System;
using System.Collections.Generic;
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

    public async Task<Plugin[]> GetAsync()
    {
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

            _contexts[plugin] = new PluginContext(
                Plugin: new Plugin(manifest, typeof())
            )
        }

        return _contexts.Values
            .Select(ctx => ctx.Plugin)
            .ToArray();
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
