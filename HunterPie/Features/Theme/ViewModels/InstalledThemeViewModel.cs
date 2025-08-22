using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private string _author = string.Empty;
    public string Author { get => _author; set => SetValue(ref _author, value); }

    private string _path = string.Empty;
    public string Path { get => _path; set => SetValue(ref _path, value); }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private bool _isDraggingOver;
    public bool IsDraggingOver { get => _isDraggingOver; set => SetValue(ref _isDraggingOver, value); }
}