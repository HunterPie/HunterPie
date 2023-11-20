using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Converter.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Settings;

#nullable enable
public static class ConfigurationPropertyProvider
{
    private static readonly Dictionary<Type, IConfigurationPropertyBuilder> Builders = new()
    {
        { typeof(string), new StringConfigurationPropertyBuilder() },
        { typeof(Enum), new EnumConfigurationPropertyBuilder() }
    };

    public static IConfigurationPropertyBuilder? FindBy(Type type)
    {
        Type? interfaceType = type.GetInterfaces()
            .FirstOrDefault(it => Builders.ContainsKey(it));

        if (interfaceType is { })
            return Builders[interfaceType];

        Type? innerType = type;

        if (type.IsGenericType)
            innerType = type.GenericTypeArguments?.FirstOrDefault();

        return innerType switch
        {
            null => null,
            { IsEnum: true } => FindBy(typeof(Enum)),
            _ => Builders.GetValueOrDefault(innerType)
        };
    }

    public static void RegisterBuilder(Type type, IConfigurationPropertyBuilder builder)
    {
        Builders.Add(type, builder);
    }
}