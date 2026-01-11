using System;

namespace HunterPie.Core.Settings.Annotations;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class ConfigurationConditionalAttribute(
    string name,
    object withValue
    ) : Attribute
{
    public string Name { get; init; } = name;
    public object WithValue { get; init; } = withValue;
}