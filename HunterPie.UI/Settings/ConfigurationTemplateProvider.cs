using HunterPie.UI.Settings.ViewModels.Internal;
using HunterPie.UI.Settings.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace HunterPie.UI.Settings;

#nullable enable
public static class ConfigurationTemplateProvider
{
    private static readonly Dictionary<Type, DataTemplate> Templates = new()
    {
        { typeof(EnumPropertyViewModel), DataTemplateFactory.Create<EnumConfigurationPropertyView>() },
        { typeof(StringPropertyViewModel), DataTemplateFactory.Create<StringConfigurationPropertyView>() }
    };

    public static DataTemplate? FindBy(Type type)
    {
        bool foundTemplate = Templates.TryGetValue(type, out DataTemplate? dataTemplate);

        return foundTemplate ? dataTemplate : null;
    }

    public static void Register(Type type, DataTemplate dataTemplate)
    {
        Templates[type] = dataTemplate;
    }
}