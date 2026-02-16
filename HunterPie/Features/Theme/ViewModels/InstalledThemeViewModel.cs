using HunterPie.Core.Client;
using HunterPie.Core.System;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeViewModel : ViewModel
{
    public required string Id { get; init; }
    public string Name { get; set => SetValue(ref field, value); } = string.Empty;
    public string Description { get; set => SetValue(ref field, value); } = string.Empty;
    public string Version { get; set => SetValue(ref field, value); } = string.Empty;
    public string Author { get; set => SetValue(ref field, value); } = string.Empty;
    public string Path { get; set => SetValue(ref field, value); } = string.Empty;
    public bool IsEnabled { get; set => SetValue(ref field, value); }
    public bool IsDraggingOver { get; set => SetValue(ref field, value); }
    public bool IsBeingDragged { get; set => SetValue(ref field, value); }

    public required ObservableCollection<string> Tags { get; init; }

    public void Toggle()
    {
        ObservableCollection<string> enabledThemes = ClientConfig.Config.Client.Themes;

        if (enabledThemes.Contains(Id))
            enabledThemes.Remove(Id);
        else
            enabledThemes.Add(Id);

        IsEnabled = enabledThemes.Contains(Id);
    }

    public void OpenFolder()
    {
        BrowserService.OpenFolder(Path);
    }
}