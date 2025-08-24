using HunterPie.Core.Client;
using HunterPie.UI.Architecture;
using System.Collections.ObjectModel;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeViewModel : ViewModel
{
    public required string Id { get; init; }

    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _description = string.Empty;
    public string Description { get => _description; set => SetValue(ref _description, value); }

    private string _version = string.Empty;
    public string Version { get => _version; set => SetValue(ref _version, value); }

    private string _author = string.Empty;
    public string Author { get => _author; set => SetValue(ref _author, value); }

    private string _path = string.Empty;
    public string Path { get => _path; set => SetValue(ref _path, value); }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private bool _isDraggingOver;
    public bool IsDraggingOver { get => _isDraggingOver; set => SetValue(ref _isDraggingOver, value); }

    private bool _isBeingDragged;
    public bool IsBeingDragged { get => _isBeingDragged; set => SetValue(ref _isBeingDragged, value); }

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
}