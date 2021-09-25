using System;

namespace HunterPie.Core.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingField : Attribute
    {
        public string Name;
        public string Description;
        public bool RequiresRestart;

        public SettingField(string name, string description, bool requiresRestart = false)
        {
            Name = name;
            Description = description;
            RequiresRestart = requiresRestart;
        }
    }
}
