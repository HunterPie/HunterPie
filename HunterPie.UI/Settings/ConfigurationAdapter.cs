using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Features.Repository;
using HunterPie.Core.Extensions;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Annotations;
using HunterPie.UI.Settings.Converter;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using ConfigurationPropertyAttribute = HunterPie.Core.Settings.Annotations.ConfigurationPropertyAttribute;

namespace HunterPie.UI.Settings;

#nullable enable
public class ConfigurationAdapter(
    IFeatureFlagRepository featureFlagRepository,
    ILocalizationRepository localizationRepository)
{
    private const string DEFAULT_SETTING_LOCALIZATION_PATH = "//Strings/Client/Settings/Setting[@Id='{0}']";
    private const string DEFAULT_CONFIGURATION_GROUP_PATH = "//Strings/Client/ConfigurationGroups/Group[@Id='{0}']";

    private readonly IFeatureFlagRepository _featureFlagRepository = featureFlagRepository;
    private readonly ILocalizationRepository _localizationRepository = localizationRepository;

    public ObservableCollection<ConfigurationCategoryGroup> Adapt<T>(T configuration, GameProcessType game = GameProcessType.None) where T : notnull
    {
        ConfigurationCategory[] categories = BuildCategoryParent(configuration.GetType(), configuration, game);

        return categories.GroupBy(it => it.CategoryGroup)
            .Select(it =>
                new ConfigurationCategoryGroup(
                    Name: it.Key,
                    Categories: it.ToObservableCollection()
                )
            ).ToObservableCollection();
    }

    private ConfigurationCategory[] BuildCategoryParent(Type parentType, object? parent, GameProcessType game)
    {
        ConfigurationAttribute? configurationAttribute = parentType.GetCustomAttribute<ConfigurationAttribute>();

        if (configurationAttribute is { })
            return BuildCategory(parentType, parent, game);

        PropertyInfo[] parentProperties = parentType.GetProperties();

        ConfigurationCategory[] AdaptCategory(PropertyInfo property)
        {
            return BuildCategory(
                categoryType: property.PropertyType,
                category: property.GetValue(parent),
                game: game
            );
        }

        return parentProperties.SelectMany(AdaptCategory)
                               .ToArray();
    }

    private ConfigurationCategory[] BuildCategory(Type categoryType, object? category, GameProcessType game)
    {
        if (category is null)
            return Array.Empty<ConfigurationCategory>();

        if (!categoryType.GetInterfaces().Contains(typeof(ISettings)))
            return Array.Empty<ConfigurationCategory>();

        ConfigurationAttribute? configurationAttribute = categoryType.GetCustomAttribute<ConfigurationAttribute>();

        if (configurationAttribute is not { })
            return BuildCategoryParent(categoryType, category, game);

        if (configurationAttribute.DependsOnFeature is { } featureFlag
            && !_featureFlagRepository.IsEnabled(featureFlag))
            return Array.Empty<ConfigurationCategory>();

        if (!configurationAttribute.AvailableGames.HasFlag(game))
            return Array.Empty<ConfigurationCategory>();

        LocalizationData localization = _localizationRepository.FindBy(DEFAULT_SETTING_LOCALIZATION_PATH.Format(configurationAttribute.Name));

        List<ConfigurationCategory> categories = new();
        List<IConfigurationProperty> configurationProperties = new();
        PropertyInfo[] properties = categoryType.GetProperties();
        var availableProperties = new Dictionary<string, (object, PropertyCondition[])>(properties.Length);

        foreach (PropertyInfo property in properties)
        {
            Type propertyType = property.PropertyType;
            object? propertyValue = property.GetValue(category);
            ConfigurationPropertyAttribute? propertyAttribute = property.GetCustomAttribute<ConfigurationPropertyAttribute>();

            if (propertyAttribute is not { })
            {
                ConfigurationCategory[] subCategories = BuildCategory(propertyType, propertyValue, game);
                categories.AddRange(subCategories);
                continue;
            }

            if (!propertyAttribute.AvailableGames.HasFlag(game))
                continue;

            if (propertyValue is null)
                continue;

            ConfigurationConditionalAttribute[] conditionAttributes = property.GetCustomAttributes<ConfigurationConditionalAttribute>()
                .Where(it => availableProperties.ContainsKey(it.Name))
                .ToArray();

            IEnumerable<PropertyCondition> conditions = conditionAttributes
                .Select(it => new PropertyCondition(
                    Property: availableProperties[it.Name].Item1,
                    Value: it.WithValue
                ));
            IEnumerable<PropertyCondition> inheritedConditions =
                conditionAttributes.SelectMany(it => availableProperties[it.Name].Item2);

            PropertyCondition[] allConditions = inheritedConditions.Concat(conditions)
                .ToArray();

            availableProperties.Add(property.Name, (propertyValue, allConditions));

            LocalizationData propertyLocalization = _localizationRepository.FindBy(DEFAULT_SETTING_LOCALIZATION_PATH.Format(propertyAttribute.Name));
            string groupLocalization = _localizationRepository.FindStringBy(DEFAULT_CONFIGURATION_GROUP_PATH.Format(propertyAttribute.Group));

            GameConfigurationAdapterAttribute? adapterAttribute =
                property.GetCustomAttribute<GameConfigurationAdapterAttribute>();

            var data = new PropertyData(
                Name: propertyLocalization.String,
                Description: propertyLocalization.Description,
                Group: groupLocalization,
                Value: propertyValue,
                Adapter: adapterAttribute?.Adapter,
                Conditions: allConditions,
                RequiresRestart: propertyAttribute.RequiresRestart
            );

            if (BuildProperty(propertyType, data, game) is not { } configurationProperty)
                continue;

            configurationProperties.Add(configurationProperty);
        }

        ObservableCollection<ConfigurationGroup> observableGroups =
            new(GroupProperties(configurationProperties));

        string categoryGroupName =
            _localizationRepository.FindStringBy(
                path: DEFAULT_CONFIGURATION_GROUP_PATH.Format(configurationAttribute.Group)
            );

        categories.Add(
            item: new ConfigurationCategory(
                Name: localization.String,
                Description: localization.Description,
                Icon: configurationAttribute.Icon,
                CategoryGroup: categoryGroupName,
                Groups: observableGroups
            )
        );

        return categories.ToArray();
    }

    private static IConfigurationProperty? BuildProperty(
        Type type,
        PropertyData data,
        GameProcessType game
    )
    {
        IConfigurationPropertyBuilder? builder = ConfigurationPropertyProvider.FindBy(type);

        return builder?.Build(data, game);
    }

    private static IEnumerable<ConfigurationGroup> GroupProperties(IEnumerable<IConfigurationProperty> configurationProperties)
    {
        IEnumerable<IGrouping<string, IConfigurationProperty>> groupedProperties = configurationProperties.GroupBy((it) => it.Group);

        return groupedProperties.Select(group =>
            new ConfigurationGroup(
                Name: group.Key,
                Properties: new ObservableCollection<IConfigurationProperty>(group)
            )
        );
    }
}