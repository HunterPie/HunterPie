using System;

namespace HunterPie.Core.Settings
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingsGroup : Attribute
    {
        public readonly string Name;
        public readonly string Description;
        public readonly string Icon;

        public SettingsGroup(
            string name,
            string description,
            string icon
        ) {
            Name = name;
            Description = description;
            Icon = icon;
        }
    }
}
