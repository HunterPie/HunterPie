using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings;

[AttributeUsage(AttributeTargets.Property)]
public class SettingField : Attribute
{
    public string Name { get; init; }
    public string Description { get; init; }
    public bool RequiresRestart { get; init; }
    public GameProcess AvailableGames { get; init; }

    public SettingField(
        string name,
        string description = null,
        bool requiresRestart = false,
        GameProcess availableGames = GameProcess.All
    )
    {
        Name = name;
        Description = description ?? $"{name}_DESC";
        RequiresRestart = requiresRestart;
        AvailableGames = availableGames;
    }
}
