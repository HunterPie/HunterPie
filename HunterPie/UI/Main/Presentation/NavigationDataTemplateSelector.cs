using HunterPie.DI;
using HunterPie.UI.Navigation.Service;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Main.Presentation;

internal class NavigationDataTemplateSelector : DataTemplateSelector
{
    private INavigationProvider Provider => DependencyContainer.Get<INavigationProvider>();

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            null => null,
            { } => Provider.FindBy(item.GetType())
        };
    }
}