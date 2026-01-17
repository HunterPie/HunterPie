using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Common;
using System;

namespace HunterPie.Core.Settings.Annotations;

#nullable enable
[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationPropertyAttribute(
    string name,
    bool requiresRestart = false,
    GameProcessType availableGames = GameProcessType.All,
    string group = CommonConfigurationGroups.MISCELLANEOUS
    ) : Attribute
{
    public string Name { get; init; } = name;
    public bool RequiresRestart { get; init; } = requiresRestart;
    public GameProcessType AvailableGames { get; init; } = availableGames;
    public string Group { get; init; } = group;
}