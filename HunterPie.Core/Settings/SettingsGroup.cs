using HunterPie.Core.Domain.Enums;
using System;

namespace HunterPie.Core.Settings
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsGroup : Attribute
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string Icon;
        public readonly string DependsOnFeature;
        public readonly GameProcess AvailableGames;

        public SettingsGroup(
            string name,
            string icon,
            string dependsOnFeature = null,
            GameProcess availableGames = GameProcess.MonsterHunterWorld | GameProcess.MonsterHunterRise | GameProcess.MonsterHunterRiseSunbreakDemo
        ) {
            Name = name;
            Description = name + "_DESC";
            Icon = icon;
            DependsOnFeature = dependsOnFeature;
            AvailableGames = availableGames;
        }
    }
}
