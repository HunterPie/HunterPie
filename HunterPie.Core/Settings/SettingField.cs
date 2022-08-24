using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingField : Attribute
    {
        public string Name;
        public string Description;
        public bool RequiresRestart;
        public readonly GameProcess AvailableGames;

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
}
