using HunterPie.Features.Extensions.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Extensions.Presentation;

internal class ThemeTabTemplateSelector : DataTemplateSelector
{
    public required DataTemplate ExploreDataTemplate { get; init; }
    public required DataTemplate InstalledDataTemplate { get; init; }
    public required DataTemplate PluginsDataTemplate { get; init; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            InstalledThemeHomeTabViewModel => InstalledDataTemplate,
            InstalledPluginsHomeTabViewModel => PluginsDataTemplate,
            ExploreThemeHomeTabViewModel => ExploreDataTemplate,
            _ => null
        };
    }
}