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

        public SettingsGroup(
            string name,
            string description,
            string icon,
            string dependsOnFeature = null
        ) {
            Name = name;
            Description = description;
            Icon = icon;
            DependsOnFeature = dependsOnFeature;
        }
    }
}
