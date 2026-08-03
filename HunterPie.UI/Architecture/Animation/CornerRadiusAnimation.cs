using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Architecture.Animation;

public class CornerRadiusAnimation : AnimationTimeline
{
    public override Type TargetPropertyType => typeof(CornerRadius);

    protected override Freezable CreateInstanceCore() => new CornerRadiusAnimation();

    public static readonly DependencyProperty TopLeftProperty =
        DependencyProperty.Register(nameof(TopLeft), typeof(double?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public double? TopLeft
    {
        get => (double?)GetValue(TopLeftProperty);
        set => SetValue(TopLeftProperty, value);
    }

    public static readonly DependencyProperty TopRightProperty =
        DependencyProperty.Register(nameof(TopRight), typeof(double?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public double? TopRight
    {
        get => (double?)GetValue(TopRightProperty);
        set => SetValue(TopRightProperty, value);
    }

    public static readonly DependencyProperty BottomRightProperty =
        DependencyProperty.Register(nameof(BottomRight), typeof(double?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public double? BottomRight
    {
        get => (double?)GetValue(BottomRightProperty);
        set => SetValue(BottomRightProperty, value);
    }

    public static readonly DependencyProperty BottomLeftProperty =
        DependencyProperty.Register(nameof(BottomLeft), typeof(double?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public double? BottomLeft
    {
        get => (double?)GetValue(BottomLeftProperty);
        set => SetValue(BottomLeftProperty, value);
    }

    public static readonly DependencyProperty FromProperty =
        DependencyProperty.Register(nameof(From), typeof(CornerRadius?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public CornerRadius? From
    {
        get => (CornerRadius?)GetValue(FromProperty);
        set => SetValue(FromProperty, value);
    }

    public static readonly DependencyProperty ToProperty =
        DependencyProperty.Register(nameof(To), typeof(CornerRadius?), typeof(CornerRadiusAnimation), new PropertyMetadata(null));

    public CornerRadius? To
    {
        get => (CornerRadius?)GetValue(ToProperty);
        set => SetValue(ToProperty, value);
    }

    public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
    {
        if (animationClock.CurrentProgress == null)
        {
            return defaultOriginValue;
        }

        double progress = animationClock.CurrentProgress.Value;
        var currentBaseValue = (CornerRadius)defaultOriginValue;

        // Determine effective From and To values, falling back to current/base values if not specified
        double startTL = From?.TopLeft ?? currentBaseValue.TopLeft;
        double endTL = TopLeft ?? To?.TopLeft ?? startTL;

        double startTR = From?.TopRight ?? currentBaseValue.TopRight;
        double endTR = TopRight ?? To?.TopRight ?? startTR;

        double startBR = From?.BottomRight ?? currentBaseValue.BottomRight;
        double endBR = BottomRight ?? To?.BottomRight ?? startBR;

        double startBL = From?.BottomLeft ?? currentBaseValue.BottomLeft;
        double endBL = BottomLeft ?? To?.BottomLeft ?? startBL;

        // Linear interpolation (Lerp) for each corner independently
        double tl = startTL + ((endTL - startTL) * progress);
        double tr = startTR + ((endTR - startTR) * progress);
        double br = startBR + ((endBR - startBR) * progress);
        double bl = startBL + ((endBL - startBL) * progress);

        return new CornerRadius(tl, tr, br, bl);
    }
}