using System;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Viewer;

public class AutoWrapPanel : Panel
{
    public double MinItemWidth
    {
        get => (double)GetValue(MinItemWidthProperty);
        set => SetValue(MinItemWidthProperty, value);
    }
    public static readonly DependencyProperty MinItemWidthProperty =
        DependencyProperty.Register(nameof(MinItemWidth), typeof(double), typeof(AutoWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double ItemHeight
    {
        get => (double)GetValue(ItemHeightProperty);
        set => SetValue(ItemHeightProperty, value);
    }
    public static readonly DependencyProperty ItemHeightProperty =
        DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(AutoWrapPanel), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    protected override Size MeasureOverride(Size availableSize)
    {
        double perLineItemCount = (int)Math.Max(1, Math.Floor(availableSize.Width / MinItemWidth));

        if (perLineItemCount <= 0)
            return availableSize;

        double totalLines = Math.Ceiling(InternalChildren.Count / perLineItemCount);
        double width = Math.Max(MinItemWidth, availableSize.Width / perLineItemCount);

        foreach (UIElement child in InternalChildren)
        {
            var size = new Size(
                width: width,
                height: ItemHeight
            );

            child.Measure(size);
        }

        return availableSize with { Height = totalLines * ItemHeight };
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Size size = base.ArrangeOverride(finalSize);

        int perLineItemCount = (int)Math.Max(1, Math.Floor(finalSize.Width / MinItemWidth));
        double width = Math.Max(MinItemWidth, finalSize.Width / perLineItemCount);

        for (int i = 0; i < InternalChildren.Count; i++)
        {
            UIElement child = InternalChildren[i];

            int column = i % perLineItemCount;
            int row = i / perLineItemCount;
            double left = width * column;
            double top = ItemHeight * row;


            var location = new Point(
                x: left,
                y: top
            );
            var itemSize = new Size(
                width: width,
                height: ItemHeight
            );

            child.Arrange(new Rect(location, itemSize));
        }

        return size;
    }
}