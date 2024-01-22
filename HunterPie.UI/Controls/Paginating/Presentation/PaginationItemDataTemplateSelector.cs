using HunterPie.UI.Controls.Paginating.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Paginating.Presentation;

#nullable enable
public class PaginationItemDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate? DefaultTemplate { get; init; }
    public DataTemplate? GapTemplate { get; init; }


    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is not PaginationItemViewModel vm)
            return null;

        return vm.IsGap switch
        {
            true => GapTemplate,
            _ => DefaultTemplate
        };
    }
}