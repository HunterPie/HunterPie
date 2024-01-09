using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings.Common;
using System;

namespace HunterPie.Core.Settings.Annotations;

#nullable enable
[AttributeUsage(AttributeTargets.Property)]
public class ConfigurationPropertyAttribute : Attribute
{
    public string Name { get; init; }
    public bool RequiresRestart { get; init; }
    public GameProcess AvailableGames { get; init; }
    public string Group { get; init; }

    public ConfigurationPropertyAttribute(
        string name,
        bool requiresRestart = false,
        GameProcess availableGames = GameProcess.All,
        string group = CommonConfigurationGroups.MISCELLANEOUS
    )
    {
        Name = name;
        RequiresRestart = requiresRestart;
        AvailableGames = availableGames;
        Group = group;
    }
}