using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Localization.Entity;
using HunterPie.Core.Domain.Features.Domain;
using HunterPie.Core.Settings.Common;
using HunterPie.UI.Settings.Converter.Model;
using HunterPie.UI.Settings.Models;
using HunterPie.UI.Settings.ViewModels.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Settings;

public class FeatureFlagAdapter(ILocalizationRepository localizationRepository)
{
    private const string FEATURES_FLAG_XPATH = "//Strings/Client/Settings/Setting[@Id='FEATURE_FLAGS_STRING']";
    private const string GENERAL_GROUP_XPATH = $"//Strings/Client/ConfigurationGroups/Group[@Id='{CommonConfigurationGroups.GENERAL}']";
    private const string FEATURE_FLAG_GROUP_XPATH = $"//Strings/Client/ConfigurationGroups/Group[@Id='{CommonConfigurationGroups.FEATURE_FLAGS}']";

    private readonly ILocalizationRepository _localizationRepository = localizationRepository;

    public ObservableCollection<ConfigurationCategoryGroup> Adapt(Dictionary<string, IFeature> flags)
    {
        ObservableCollection<IConfigurationProperty> properties = new();
        string categoryName = _localizationRepository.FindStringBy(GENERAL_GROUP_XPATH);

        foreach ((string name, IFeature feature) in flags)
        {
            var property = new BooleanPropertyViewModel(feature.IsEnabled)
            {
                Name = name,
                Description = "",
                Group = categoryName,
                RequiresRestart = true,
                Conditions = Array.Empty<PropertyCondition>(),
            };

            properties.Add(property);
        }

        var group = new ConfigurationGroup(
            Name: categoryName,
            Properties: properties
        );

        LocalizationData localizationData = _localizationRepository.FindBy(FEATURES_FLAG_XPATH);

        string categoryGroup = _localizationRepository.FindStringBy(FEATURE_FLAG_GROUP_XPATH);

        var category = new ConfigurationCategory(
            Name: localizationData.String,
            Description: localizationData.Description,
            Icon: "ICON_FLAG",
            CategoryGroup: categoryGroup,
            Groups: new ObservableCollection<ConfigurationGroup> { group }
        );

        return new ObservableCollection<ConfigurationCategoryGroup>
        {
            new (
                Name: categoryGroup,
                Categories: new ObservableCollection<ConfigurationCategory> { category }
            )
        };
    }
}