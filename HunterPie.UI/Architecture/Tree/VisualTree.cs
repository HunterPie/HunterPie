using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Tree;

#nullable enable
public static class VisualTree
{
    public static T? TryFindParent<T>(this DependencyObject child)
        where T : DependencyObject
    {
        DependencyObject? parentObject = GetParentObject(child);

        return parentObject switch
        {
            null => null,

            T parent => parent,

            _ => TryFindParent<T>(parentObject)
        };
    }

    public static DependencyObject? GetParentObject(this DependencyObject child)
    {
        switch (child)
        {
            case null:
                return null;

            case ContentElement contentElement:
                {
                    DependencyObject parent = ContentOperations.GetParent(contentElement);

                    if (parent is not null)
                        return parent;

                    var fce = contentElement as FrameworkContentElement;
                    return fce?.Parent;
                }
        }

        if (child is not FrameworkElement frameworkElement)
            return VisualTreeHelper.GetParent(child);

        return frameworkElement.Parent ?? VisualTreeHelper.GetParent(child);
    }
}