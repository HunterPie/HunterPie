using HunterPie.Core.Observability.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace HunterPie.Features.Theme.Loader;

internal class ThemeLoaderService
{
    private const string XAML_PATTERN = "*.xaml";
    private readonly ILogger _logger = LoggerFactory.Create();
    private readonly List<ResourceDictionary> _loadedFiles = new();

    private readonly Application _application;

    private FileSystemWatcher? _watcher;

    public ThemeLoaderService(Application application)
    {
        _application = application;
    }

    public void Load(string path)
    {
        _watcher?.Dispose();

        _watcher = new FileSystemWatcher(path)
        {
            Filter = XAML_PATTERN,
            NotifyFilter = NotifyFilters.LastWrite,
            EnableRaisingEvents = true,
            IncludeSubdirectories = false
        };
        _watcher.Changed += OnFileChanged;
    }

    private void OnFileChanged(object sender, FileSystemEventArgs e)
    {
        string? directory = Directory.GetParent(e.FullPath)?.FullName;

        if (directory is null)
            return;

        LoadTheme(directory);
    }

    private void LoadTheme(string path)
    {
        IEnumerable<string> themeFiles = Directory.EnumerateFiles(
            path: path,
            searchPattern: XAML_PATTERN
        );

        foreach (string file in themeFiles)
        {
            bool success = TryLoadResource(file);

            if (!success)
                continue;

            _logger.Debug($"Loaded file {Path.GetFileName(file)}");
        }
    }

    private bool TryLoadResource(string file)
    {
        try
        {
            using var stream = new FileStream(
                path: file,
                mode: FileMode.Open,
                access: FileAccess.Read
            );

            var resource = (ResourceDictionary)XamlReader.Load(stream);
            _application.Resources.MergedDictionaries.Add(resource);
            _loadedFiles.Add(resource);

            return true;
        }
        catch (Exception err)
        {
            _logger.Error($"Failed to load theme file {file}\n{err}");

            return false;
        }
    }

    private void UnloadResources()
    {
        foreach (ResourceDictionary resource in _loadedFiles)
            _application.Resources.MergedDictionaries.Remove(resource);
    }
}