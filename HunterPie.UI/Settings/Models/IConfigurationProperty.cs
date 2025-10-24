using HunterPie.UI.Settings.Converter.Model;
using System.Collections.Generic;

namespace HunterPie.UI.Settings.Models;

#nullable enable
public interface IConfigurationProperty
{
    public string Name { get; }
    public string Description { get; }
    public string Group { get; }
    public bool RequiresRestart { get; }
    public IReadOnlyCollection<PropertyCondition> Conditions { get; init; }
}