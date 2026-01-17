using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Extensions;
using HunterPie.Core.Observability.Logging;
using HunterPie.Features.Theme.Entity;
using HunterPie.Features.Theme.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.Features.Theme.Loader;

internal class ThemeLoaderService(
    Application application,
    V5Config config,
    LocalThemeRepository localThemeRepository)
{
    private const string XAML_PATTERN = "*.xaml";
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly Dictionary<string, List<ResourceDictionary>> _loadedFiles = new();

    private readonly Application _application = application;
    private readonly V5Config _config = config;
    private readonly LocalThemeRepository _localThemeRepository = localThemeRepository;

    private FileSystemWatcher? _watcher;

    public async Task LoadAsync()
    {
        _watcher?.Dispose();

        _watcher = new FileSystemWatcher(ClientInfo.ThemesPath)
        {
            Filter = XAML_PATTERN,
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true,
            IncludeSubdirectories = true
        };
        Action reloadThemes = ReloadAllThemes;
        Action callback = reloadThemes.Debounce();
        _watcher.Changed += (_, __) => callback();

        await LoadAllEnabledThemesAsync();
    }

    public async Task LoadAllEnabledThemesAsync()
    {
        IReadOnlyCollection<LocalThemeManifest> themes = await _localThemeRepository.ListAllAsync();

        IOrderedEnumerable<LocalThemeManifest> enabledThemes = themes
            .Where(it => _config.Client.Themes.Contains(it.Manifest.Id))
            .OrderByDescending(it => _config.Client.Themes.IndexOf(it.Manifest.Id));

        foreach (LocalThemeManifest theme in enabledThemes)
            LoadTheme(theme);
    }

    public void UnloadAllThemes()
    {
        foreach (string themeId in _loadedFiles.Keys)
        {
            LocalThemeManifest? manifest = _localThemeRepository.FindBy(themeId);

            if (manifest is null)
                continue;

            UnloadTheme(manifest);
        }
    }

    private void UnloadTheme(LocalThemeManifest manifest)
    {
        if (!_loadedFiles.TryGetValue(manifest.Manifest.Id, out List<ResourceDictionary>? resources))
            return;

        foreach (ResourceDictionary resource in resources)
            _application.Resources.MergedDictionaries.Remove(resource);

        _loadedFiles.Remove(manifest.Manifest.Id);

        _logger.Info($"Unloaded theme {manifest.Manifest.Name}");
    }

    private void LoadTheme(LocalThemeManifest manifest)
    {
        IEnumerable<string> themeFiles = Directory.EnumerateFiles(
            path: manifest.Path,
            searchPattern: XAML_PATTERN
        );

        int loadedFiles = 0;
        int totalFiles = 0;
        foreach (string file in themeFiles)
        {
            totalFiles++;
            bool success = TryLoadResource(manifest.Manifest, file);

            if (!success)
                continue;

            _logger.Debug($"Loaded file {Path.GetFileName(file)}");
            loadedFiles++;
        }

        string themeIdentifier = $"{manifest.Manifest.Name} @ {manifest.Manifest.Version}";

        if (loadedFiles == totalFiles)
            _logger.Info($"Loaded theme {themeIdentifier}");
        else if (loadedFiles < totalFiles)
            _logger.Warning($"Loaded theme partially {themeIdentifier}");
        else
            _logger.Error($"Failed to load theme {themeIdentifier}");
    }

    private bool TryLoadResource(ThemeManifest manifest, string filePath)
    {
        List<ResourceDictionary> resources = _loadedFiles.ContainsKey(manifest.Id)
            ? _loadedFiles[manifest.Id]
            : new List<ResourceDictionary>();

        try
        {
            using var stream = new FileStream(
                path: filePath,
                mode: FileMode.Open,
                access: FileAccess.Read
            );

            var resource = (ResourceDictionary)XamlReader.Load(stream);
            _application.Resources.MergedDictionaries.Add(resource);
            resources.Add(resource);

            if (!_loadedFiles.ContainsKey(manifest.Id))
                _loadedFiles.Add(manifest.Id, resources);

            return true;
        }
        catch (Exception err)
        {
            _logger.Error($"[{manifest.Name} @ {manifest.Version}] Failed to load theme file {filePath}\n{err}");

            return false;
        }
    }

    private async void ReloadAllThemes()
    {
        UnloadAllThemes();
        await LoadAllEnabledThemesAsync();
    }
}