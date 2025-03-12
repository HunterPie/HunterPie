using HunterPie.UI.Architecture.Tree;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Display.Presentation;

#nullable enable
public class ConditionalTemplateSelector : DataTemplateSelector
{
    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is not bool isEnabled)
            return null;

        Conditional? conditionalUi = container.TryFindParent<Conditional>();

        if (conditionalUi is null)
            return null;

        return isEnabled
            ? conditionalUi.Component
            : conditionalUi.Otherwise;
    }
}