using HunterPie.UI.Settings.Converter;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.UI.Settings;

#nullable enable
public static class ConfigurationViewProvider
{
    private static readonly Dictionary<Type, IConfigurationViewBuilder> Builders = new();

    public static IConfigurationViewBuilder? FindBy(Type type)
    {
        Type? interfaceType = type.GetInterfaces()
            .FirstOrDefault(it => Builders.ContainsKey(it));

        if (interfaceType is { })
            return Builders[interfaceType];

        Type? innerType = type;

        // Gets inner type for types with generic arguments
        if (type.IsGenericType)
            innerType = type.GenericTypeArguments?.FirstOrDefault();

        return innerType switch
        {
            null => null,
            { IsEnum: true } => FindBy(typeof(Enum)),
            _ => Builders.GetValueOrDefault(innerType)
        };
    }

    public static void RegisterBuilder(Type type, IConfigurationViewBuilder builder)
    {
        Builders.Add(type, builder);
    }
}