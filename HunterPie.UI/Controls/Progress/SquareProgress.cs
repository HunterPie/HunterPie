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

    private Geometry GetGeometry()
    {
        double percentage = Math.Clamp(Current / Math.Max(1, Max), 0, 1);
        double width = Math.Max(0, RenderSize.Width);
        double height = Math.Max(0, RenderSize.Height);
        double radius = Math.Min(Radius, Math.Min(width, height) / 2.0);

        double side = Math.Min(width, height) - (2 * radius);
        double halfSide = side / 2;
        double arcLength = Math.PI * radius / 2;
        double totalLength = (4 * side) + (4 * arcLength);
        double targetLength = percentage * totalLength;
        double remaining = targetLength;

        var current = new Point(width / 2, 0);
        var figure = new PathFigure { StartPoint = current, IsFilled = false };

        void LineOrPartial(Point to, double length)
        {
            if (remaining <= 0)
                return;

            if (remaining < length)
            {
                Vector dir = to - current;
                dir.Normalize();
                Point mid = current + (dir * remaining);
                figure.Segments.Add(new LineSegment(mid, true));
                remaining = 0;
                current = mid;
            }
            else
            {
                figure.Segments.Add(new LineSegment(to, true));
                remaining -= length;
                current = to;
            }
        }

        void ArcTo(Point center, int quadrant, SweepDirection dir)
        {
            if (remaining <= 0)
                return;

            double fullAngle = Math.PI / 2; // 90 degrees
            double angleToSweep = Math.Min(fullAngle, remaining / arcLength * fullAngle);
            remaining -= angleToSweep * radius;

            // Compute start angle based on quadrant
            double startAngle = quadrant switch
            {
                0 => 270,
                1 => 0,
                2 => 90,
                3 => 180,
                _ => 0
            };

            if (dir == SweepDirection.Counterclockwise)
                angleToSweep *= -1;

            double radians = (startAngle + (angleToSweep * 180 / Math.PI)) * Math.PI / 180;

            var arcEnd = new Point(
                center.X + (radius * Math.Cos(radians)),
                center.Y + (radius * Math.Sin(radians))
            );

            bool isLargeArc = Math.Abs(angleToSweep) > Math.PI / 2;

            figure.Segments.Add(new ArcSegment(arcEnd, new Size(radius, radius), angleToSweep * 180 / Math.PI, isLargeArc, dir, true));
            current = arcEnd;
        }

        var p1 = new Point(width - radius, 0);

        var p2 = new Point(width, height - radius);

        var p3 = new Point(radius, height);

        var p4 = new Point(0, radius);

        var p5 = new Point((width / 2) + radius, 0);

        LineOrPartial(p1, halfSide);
        ArcTo(new Point(width - radius, radius), 0, SweepDirection.Clockwise);

        LineOrPartial(p2, side);
        ArcTo(new Point(width - radius, height - radius), 1, SweepDirection.Clockwise);

        LineOrPartial(p3, side);
        ArcTo(new Point(radius, height - radius), 2, SweepDirection.Clockwise);

        LineOrPartial(p4, side);
        ArcTo(new Point(radius, radius), 3, SweepDirection.Clockwise);

        LineOrPartial(p5, halfSide);

        var geometry = new PathGeometry();
        geometry.Figures.Add(figure);
        return geometry;
    }
}