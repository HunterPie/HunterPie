using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings.Annotations;

[AttributeUsage(AttributeTargets.Class)]
public class ConfigurationAttribute(
    string name,
    string icon,
    string group,
    string? dependsOnFeature = null,
    GameProcessType availableGames = GameProcessType.All
    ) : Attribute
{
    public string Name { get; init; } = name;
    public string Icon { get; init; } = icon;
    public string Group { get; init; } = group;
    public string? DependsOnFeature { get; init; } = dependsOnFeature;
    public GameProcessType AvailableGames { get; init; } = availableGames;
}