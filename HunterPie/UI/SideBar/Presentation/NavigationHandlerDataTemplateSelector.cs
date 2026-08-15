using HunterPie.UI.Client.Sidebar.Handler;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.SideBar.Presentation;

internal class NavigationHandlerDataTemplateSelector : DataTemplateSelector
{
    public required DataTemplate GroupDataTemplate { get; init; }

    public required DataTemplate DefaultDataTemplate { get; init; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
        return item switch
        {
            NavigationHandler.Group => GroupDataTemplate,
            _ => DefaultDataTemplate
        };
    }

}