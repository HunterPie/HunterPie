using HunterPie.Core.Client;
using HunterPie.Core.Extensions;
using HunterPie.Features.Theme.Entity;
using HunterPie.Features.Theme.Repository;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeHomeTabViewModel(
    ObservableCollection<string> configuredThemes,
    LocalThemeRepository localThemeRepository) : ThemeHomeTabViewModel
{
    private readonly ObservableCollection<string> _configuredThemes = configuredThemes;
    private readonly LocalThemeRepository _localThemeRepository = localThemeRepository;

    public ObservableCollection<InstalledThemeViewModel> Themes { get; } = new();
    public bool IsRefreshing { get; set => SetValue(ref field, value); }

    public void Arrange(InstalledThemeViewModel source, InstalledThemeViewModel target)
    {
        int fromIndexLogical = _configuredThemes.IndexOf(source.Id);
        int toIndexLogical = _configuredThemes.IndexOf(target.Id);

        if (fromIndexLogical >= 0 && toIndexLogical >= 0)
            _configuredThemes.Move(fromIndexLogical, toIndexLogical);

        int fromIndexVirtual = Themes.IndexOf(source);
        int toIndexVirtual = Themes.IndexOf(target);
        Themes.Move(fromIndexVirtual, toIndexVirtual);

        Sort();
    }

    public void Sort()
    {
        IsRefreshing = true;

        IEnumerable<InstalledThemeViewModel> enabledThemes = _configuredThemes.Select(id => Themes.FirstOrDefault(theme => theme.Id == id))
            .FilterNull();

        IEnumerable<InstalledThemeViewModel> disabledThemes = Themes.Where(it => !it.IsEnabled);

        InstalledThemeViewModel[] elements = enabledThemes
            .Concat(disabledThemes)
            .ToArray();

        Themes.Clear();

        foreach (InstalledThemeViewModel element in elements)
            Themes.Add(element);

        // Artificially simulate loading time because it feels very weird to have a very quick response
        Task.Delay(300)
            .ContinueWith(_ => IsRefreshing = false);
    }

    public async Task InstallTheme()
    {
        var dialog = new OpenFileDialog
        {
            DefaultExt = ".zip",
            Filter = "ZIP files (*.zip)|*.zip"
        };

        bool? result = dialog.ShowDialog();

        if (result != true)
            return;

        string fileName = dialog.FileName;

        ZipFile.ExtractToDirectory(
            sourceArchiveFileName: fileName,
            destinationDirectoryName: ClientInfo.ThemesPath,
            overwriteFiles: true
        );

        await RefreshAsync();
    }

    public async Task RefreshAsync()
    {
        Themes.Clear();

        IReadOnlyCollection<LocalThemeManifest> localThemes = await _localThemeRepository.ListAllAsync();

        foreach (LocalThemeManifest theme in localThemes)
            Themes.Add(new InstalledThemeViewModel
            {
                Id = theme.Manifest.Id,
                Name = theme.Manifest.Name,
                Description = theme.Manifest.Description,
                Author = theme.Manifest.Author,
                Version = theme.Manifest.Version,
                Path = theme.Path,
                IsEnabled = _configuredThemes.Contains(theme.Manifest.Id),
                IsDraggingOver = false,
                Tags = theme.Manifest.Tags.ToObservableCollection()
            });

        Sort();
    }
}