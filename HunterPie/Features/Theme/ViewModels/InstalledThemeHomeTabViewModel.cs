using HunterPie.Core.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeHomeTabViewModel : ThemeHomeTabViewModel
{
    private readonly ObservableCollection<string> _configuredThemes;

    public ObservableCollection<InstalledThemeViewModel> Themes { get; } = new();

    public InstalledThemeHomeTabViewModel(ObservableCollection<string> configuredThemes)
    {
        _configuredThemes = configuredThemes;
    }

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
        IEnumerable<InstalledThemeViewModel> enabledThemes = _configuredThemes.Select(id => Themes.FirstOrDefault(theme => theme.Id == id))
            .FilterNull();

        IEnumerable<InstalledThemeViewModel> disabledThemes = Themes.Where(it => !it.IsEnabled);

        InstalledThemeViewModel[] elements = enabledThemes
            .Concat(disabledThemes)
            .ToArray();

        Themes.Clear();

        foreach (InstalledThemeViewModel element in elements)
            Themes.Add(element);
    }
}