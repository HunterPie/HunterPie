using HunterPie.DI;
using HunterPie.UI.Navigation.Service;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Main.Presentation;

public class MainBodyDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? Default { get; init; }

    private INavigationProvider Provider => DependencyContainer.Get<INavigationProvider>();

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        return item switch
        {
            null => Default,
            _ => Provider.FindBy(item.GetType()),
        };
    }
}