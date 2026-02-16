using HunterPie.UI.Architecture;

namespace HunterPie.Features.Settings.ViewModels;

internal class ConfigurationCategoryTitle : ViewModel, IConfigurationCategory
{
    public string Title { get; set => SetValue(ref field, value); } = string.Empty;
}