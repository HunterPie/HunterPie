using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Controls.Viewer;
public class SmoothScrollViewer : ScrollViewer
{
    private double _totalVerticalOffset;
    private readonly Queue<double> _verticalOffsetQueue = new();
    private bool _isScrolling;

    public double CurrentVerticalOffset
    {
        get => (double)GetValue(CurrentVerticalOffsetProperty);
        set => SetValue(CurrentVerticalOffsetProperty, value);
    }

    // Using a DependencyProperty as the backing store for CurrentVerticalOffset.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentVerticalOffsetProperty =
        DependencyProperty.Register("CurrentVerticalOffset", typeof(double), typeof(ScrollViewer), new PropertyMetadata(0.0, OnCurrentVerticalOffsetChanged));

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        e.Handled = true;

        if (!_isScrolling)
        {
            _totalVerticalOffset = VerticalOffset;
            CurrentVerticalOffset = VerticalOffset;
        }

        double x = _totalVerticalOffset - (e.Delta / 2);
        _totalVerticalOffset = Math.Min(Math.Max(0, x), ScrollableHeight);
        AnimateVerticalScrolling(_totalVerticalOffset);
    }

    private static void OnCurrentVerticalOffsetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ScrollViewer sv && e.NewValue is double value)
            sv.ScrollToVerticalOffset(value);
    }

    private void AnimateVerticalScrolling(double offset, double duration = 500)
    {
        _verticalOffsetQueue.Enqueue(offset);
        var animation = new DoubleAnimation(offset, TimeSpan.FromMilliseconds(duration))
        {
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut },
            FillBehavior = FillBehavior.Stop
        };
        animation.Completed += OnAnimationCompleted;
        _isScrolling = true;

        BeginAnimation(CurrentVerticalOffsetProperty, animation, HandoffBehavior.Compose);
    }

    private void OnAnimationCompleted(object sender, EventArgs e)
    {
        if (sender is Timeline tl)
            tl.Completed -= OnAnimationCompleted;

        CurrentVerticalOffset = _verticalOffsetQueue.Dequeue();
        _isScrolling = false;
    }
}
