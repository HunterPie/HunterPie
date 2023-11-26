using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Models;

namespace HunterPie.UI.Settings.ViewModels;

public class ConfigurationPropertyViewModel : ViewModel, IConfigurationProperty
{
    private bool _isMatch = true;

    public string Name { get; init; }
    public string Description { get; init; }
    public string Group { get; init; }
    public bool RequiresRestart { get; init; }

    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }
}