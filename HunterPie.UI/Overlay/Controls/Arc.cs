using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HunterPie.UI.Overlay.Controls;

public class Arc : Shape
{

    /*
        Credits: https://stackoverflow.com/questions/36752183/wpf-doughnut-progressbar
    */

    public double StartAngle
    {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }
    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(Arc), new FrameworkPropertyMetadata(90.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public bool Reverse
    {
        get => (bool)GetValue(ReverseProperty);
        set => SetValue(ReverseProperty, value);
    }
    public static readonly DependencyProperty ReverseProperty =
        DependencyProperty.Register(nameof(Reverse), typeof(bool), typeof(Arc), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

    public double Percentage
    {
        get => (double)GetValue(PercentageProperty);
        set => SetValue(PercentageProperty, value);
    }
    public static readonly DependencyProperty PercentageProperty =
        DependencyProperty.Register(nameof(Percentage), typeof(double), typeof(Arc), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double EndAngle
    {
        get => (double)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }
    public static readonly DependencyProperty EndAngleProperty =
        DependencyProperty.Register(nameof(EndAngle), typeof(double), typeof(Arc), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    protected override Geometry DefiningGeometry => GetArcGeometry();

    private Geometry GetArcGeometry()
    {
        double percentage = Math.Clamp(
            value: Percentage,
            min: 0.0,
            max: 1.0
        );
        double end = ConvertPercentageIntoAngle(Reverse, percentage);

        var middlePoint = new Point(
            x: (RenderSize.Width - StrokeThickness) / 2,
            y: (RenderSize.Height - StrokeThickness) / 2
        );
        Point realStartPoint = PointAtAngle(Math.Min(StartAngle, end));
        Point endPoint = PointAtAngle(Math.Max(StartAngle, end));

        var arcSize = new Size(
            width: Math.Max(0, middlePoint.X),
            height: Math.Max(0, middlePoint.Y)
        );
        bool isLargeArc = Math.Abs(end - StartAngle) > 180;
        var geom = new StreamGeometry();
        using (StreamGeometryContext context = geom.Open())
        {
            context.BeginFigure(
                startPoint: middlePoint,
                isFilled: true,
                isClosed: false
            );
            context.LineTo(
                point: realStartPoint,
                isStroked: false,
                isSmoothJoin: false
            );
            context.ArcTo(
                point: endPoint,
                size: arcSize,
                rotationAngle: 0,
                isLargeArc: isLargeArc,
                sweepDirection: SweepDirection.Counterclockwise,
                isStroked: true,
                isSmoothJoin: true
            );
        }

        geom.Transform = new TranslateTransform(
            offsetX: StrokeThickness / 2,
            offsetY: StrokeThickness / 2
        );
        return geom;
    }

    private Point PointAtAngle(double angle)
    {
        double radAngle = angle * (Math.PI / 180);
        double xRadius = (RenderSize.Width - StrokeThickness) / 2;
        double yRadius = (RenderSize.Height - StrokeThickness) / 2;
        double x = xRadius + (xRadius * Math.Cos(radAngle));
        double y = yRadius - (yRadius * Math.Sin(radAngle));
        return new Point(x, y);
    }

    public static double ConvertPercentageIntoAngle(bool reverse, double percentage)
    {
        double angle = reverse switch
        {
            true => 90.0 + (360.0 * percentage),
            false => 90.0 - (360.0 * percentage),
        };
        double cap = -269.999;
        return Math.Max(angle, cap);
    }
}