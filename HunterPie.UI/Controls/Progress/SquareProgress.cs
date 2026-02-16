using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HunterPie.UI.Controls.Progress;

public class SquareProgress : Shape
{
    public double Current
    {
        get => (double)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    // Using a DependencyProperty as the backing store for Current.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentProperty =
        DependencyProperty.Register(nameof(Current), typeof(double), typeof(SquareProgress), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register(nameof(Max), typeof(double), typeof(SquareProgress), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius), typeof(double), typeof(SquareProgress), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender));

    protected override Geometry DefiningGeometry => GetGeometry();

    private StreamGeometry GetGeometry()
    {
        double percentage = Math.Clamp(Current / Math.Max(1, Max), 0, 1);
        double width = Math.Max(0, RenderSize.Width) - StrokeThickness;
        double height = Math.Max(0, RenderSize.Height) - StrokeThickness;
        double radius = Math.Min(Radius, Math.Min(width, height) / 2.0);

        double side = Math.Min(width, height) - (2 * radius);
        double halfSide = (side / 2) + radius;
        double arcLength = Math.PI * radius / 2;
        double totalLength = (4 * side) + (4 * arcLength);
        double targetLength = percentage * totalLength;
        double remaining = targetLength;

        var current = new Point(halfSide, 0);

        var geom = new StreamGeometry();
        using StreamGeometryContext context = geom.Open();

        context.BeginFigure(
            startPoint: current,
            isFilled: true,
            isClosed: false
        );

        void LineOrPartial(Point to, double length)
        {
            if (remaining <= 0)
                return;

            bool isPartial = remaining < length;

            if (isPartial)
            {
                Vector direction = to - current;
                direction.Normalize();

                Point middle = current + (direction * remaining);
                context.LineTo(
                    point: middle,
                    isStroked: true,
                    isSmoothJoin: false
                );

                remaining = 0;
                current = middle;
                return;
            }

            context.LineTo(
                point: to,
                isStroked: true,
                isSmoothJoin: false
            );
            remaining -= length;
            current = to;
        }

        void ArcTo(Point center, int quadrant, SweepDirection dir)
        {
            if (remaining <= 0)
                return;

            const double fullAngle = Math.PI / 2;
            double angleToSweep = Math.Min(fullAngle, remaining / arcLength * fullAngle);
            remaining -= angleToSweep * radius;

            double startAngle = 270 + (quadrant * 90);

            if (dir == SweepDirection.Counterclockwise)
                angleToSweep *= -1;

            double radians = (startAngle + (angleToSweep * 180 / Math.PI)) * Math.PI / 180;

            var arcEnd = new Point(
                x: center.X + (radius * Math.Cos(radians)),
                y: center.Y + (radius * Math.Sin(radians))
            );

            bool isLargeArc = Math.Abs(angleToSweep) > Math.PI / 2;
            var size = new Size(
                width: radius,
                height: radius
            );

            context.ArcTo(
                point: arcEnd,
                size: size,
                rotationAngle: angleToSweep * 180 / Math.PI,
                isLargeArc: isLargeArc,
                sweepDirection: dir,
                isStroked: true,
                isSmoothJoin: false
            );
            current = arcEnd;
        }

        var rightTop = new Point(width - radius, 0);
        var topRightCorner = new Point(width - radius, radius);

        var rightSide = new Point(width, height - radius);
        var bottomRightCorner = new Point(width - radius, height - radius);

        var bottom = new Point(radius, height);
        var bottomLeftCorner = new Point(radius, height - radius);

        var leftSide = new Point(0, radius);
        var topLeftCorner = new Point(radius, radius);

        var leftTop = new Point((width / 2), 0);

        LineOrPartial(rightTop, halfSide - radius);
        ArcTo(topRightCorner, 0, SweepDirection.Clockwise);

        LineOrPartial(rightSide, side);
        ArcTo(bottomRightCorner, 1, SweepDirection.Clockwise);

        LineOrPartial(bottom, side);
        ArcTo(bottomLeftCorner, 2, SweepDirection.Clockwise);

        LineOrPartial(leftSide, side);
        ArcTo(topLeftCorner, 3, SweepDirection.Clockwise);

        LineOrPartial(leftTop, halfSide);

        bool isPartial = targetLength < totalLength;
        if (isPartial && Fill != null)
        {
            double middlePoint = Math.Min(width, height) / 2;
            var middle = new Point(middlePoint, middlePoint);
            context.LineTo(middle, false, true);
        }

        geom.Transform = new TranslateTransform(
            offsetX: StrokeThickness / 2,
            offsetY: StrokeThickness / 2
        );

        return geom;
    }
}