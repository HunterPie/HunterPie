using HunterPie.UI.Architecture;

namespace HunterPie.Features.Theme.ViewModels;

internal class InstalledThemeViewModel : ViewModel
{
    private string _name = string.Empty;
    public string Name { get => _name; set => SetValue(ref _name, value); }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }
}