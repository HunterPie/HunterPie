using HunterPie.Features.Theme.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Theme.Presentation;

internal class ThemeTabTemplateSelector : DataTemplateSelector
{
    public required DataTemplate ExploreDataTemplate { get; init; }
    public required DataTemplate InstalledDataTemplate { get; init; }

    public override DataTemplate? SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            InstalledThemeHomeTabViewModel => InstalledDataTemplate,
            ExploreThemeHomeTabViewModel => ExploreDataTemplate,
            _ => null
        };
    }
}