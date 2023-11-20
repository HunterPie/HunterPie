using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings.Annotations;

#nullable enable

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute : Attribute
{
    public string Name { get; init; }
    public string Icon { get; init; }
    public string? DependsOnFeature { get; init; }
    public GameProcess AvailableGames { get; init; }

    public ConfigurationAttribute(
        string name,
        string icon,
        string? dependsOnFeature = null,
        GameProcess availableGames = GameProcess.All
    )
    {
        Name = name;
        Icon = icon;
        DependsOnFeature = dependsOnFeature;
        AvailableGames = availableGames;
    }
}