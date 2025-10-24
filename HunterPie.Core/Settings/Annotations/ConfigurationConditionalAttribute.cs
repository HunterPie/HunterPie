using System;

namespace HunterPie.Core.Settings.Annotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConfigurationConditionalAttribute : Attribute
{
    public string Name { get; init; }
    public object WithValue { get; init; }

    public ConfigurationConditionalAttribute(
        string name,
        object withValue
    )
    {
        Name = name;
        WithValue = withValue;
    }
}