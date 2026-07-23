using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Json;
using HunterPie.Core.Observability.Logging;
using HunterPie.Core.Plugins.DI;
using HunterPie.Core.Plugins.Entity;
using HunterPie.Features.Languages.Repository;
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

internal class LocalPluginRepository(
    ILocalizationRegistry localizationRegistry,
    IConfiguration configuration
) : IPluginRepository
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
            PluginContext? context = await CreatePluginContext(plugin);

            if (context is not { } ctx)
                continue;

            _contexts[plugin] = ctx;
        }
    }

    public IReadOnlyList<PluginContext> FindAll()
    {
        return _contexts.Values.ToImmutableArray();
    }

    private async Task<PluginContext?> CreatePluginContext(string pluginPath)
    {
        string[] assemblies = Directory.EnumerateFiles(pluginPath, "*.dll")
                .ToArray();

        if (assemblies.Length == 0)
            return null;

        PluginManifest? manifest = await TryLoadManifestAsync(pluginPath);

        if (manifest is not { })
            return null;

        var context = new PluginLoadContext(pluginPath);

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
                return null;
            }
        }

        Type? moduleType = context.Assemblies.SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => typeof(IPluginModule).IsAssignableFrom(t) && !t.IsAbstract);

        if (moduleType is not { })
        {
            _logger.Warning($"No module found for plugin '{manifest.Name}'");
            return null;
        }

        var module = Activator.CreateInstance(moduleType) as IPluginModule;

        if (module is not { })
        {
            _logger.Warning($"Failed to create instance of module '{moduleType.FullName}' for plugin '{manifest.Name}'");
            return null;
        }

        string configPath = Path.Join(pluginPath, "configuration.json");

        BindConfiguration(
            path: configPath,
            instance: module.Configuration
        );

        LoadLocalization(
            pluginPath: pluginPath
        );

        Type? pluginType = context.Assemblies.SelectMany(it => it.GetTypes())
            .FirstOrDefault(it => typeof(IPlugin).IsAssignableFrom(it) && !it.IsAbstract);

        if (pluginType is null)
        {
            _logger.Warning($"No plugin type found for plugin '{manifest.Name}'");
            return null;
        }

        return new PluginContext(
            Plugin: new Plugin(
                Manifest: manifest,
                Configuration: module.Configuration,
                Type: pluginType
            ),
            Module: module,
            Context: context
        );
    }

    private static async Task<PluginManifest?> TryLoadManifestAsync(string path)
    {
        string manifestPath = Path.Combine(path, "plugin.manifest.json");

        if (!File.Exists(manifestPath))
            return null;

        string manifestContent = await File.ReadAllTextAsync(manifestPath);

        return JsonProvider.Deserializer<PluginManifest>(manifestContent);
    }

    private static void BindConfiguration(string path, object instance)
    {
        ConfigManager.Register(
                path: path,
                @default: instance
            );
        ConfigManager.BindConfiguration(
            path: path,
            data: instance
        );
    }

    private void LoadLocalization(string pluginPath)
    {
        string pluginLocalizationFolder = Path.Combine(pluginPath, "Languages");

        if (!Directory.Exists(pluginLocalizationFolder))
            return;

        string currentLocalization = configuration.Client.Language.Current;
        string currentLocalizationFile = Path.Combine(pluginLocalizationFolder, currentLocalization);

        if (File.Exists(currentLocalizationFile))
        {
            LoadLocalizationFile(currentLocalizationFile);
            return;
        }

        string defaultLocalizationFile = Path.Combine(pluginLocalizationFolder, "en-us.xml");
        if (!File.Exists(defaultLocalizationFile))
            return;

        LoadLocalizationFile(defaultLocalizationFile);
    }

    private void LoadLocalizationFile(string filepath)
    {
        try
        {
            localizationRegistry.Register(filepath);
            _logger.Debug($"loaded plugin localization: {filepath}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Failed to load plugin localization file: {ex}");
        }

        return;
    }
}