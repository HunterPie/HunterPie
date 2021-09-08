using System;

namespace HunterPie.Core.Settings
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingField : Attribute
    {
        public string Name;
        public string Description;

        public SettingField(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
