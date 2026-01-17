using HunterPie.UI.Architecture;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using System.Collections.Generic;

namespace HunterPie.UI.Settings.ViewModels;

#nullable enable
public class ConfigurationPropertyViewModel : ViewModel, IConfigurationProperty
{
    public required string Name { get; init; } = string.Empty;
    public required string Description { get; init; } = string.Empty;
    public required string Group { get; init; } = string.Empty;
    public required bool RequiresRestart { get; init; }
    public required IReadOnlyCollection<PropertyCondition> Conditions { get; init; }

    public bool IsMatch { get; set => SetValue(ref field, value); } = true;
}