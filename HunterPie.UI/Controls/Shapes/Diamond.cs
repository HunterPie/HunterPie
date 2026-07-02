using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HunterPie.UI.Controls.Shapes;

public class Diamond : Shape
{
    protected override Geometry DefiningGeometry => GetGeometry();

    private Geometry GetGeometry()
    {
        double actualWidth = RenderSize.Width - StrokeThickness;
        double actualHeight = RenderSize.Height - StrokeThickness;

        var startingPoint = new Point(
            x: actualWidth * 0.5,
            y: 0
        );
        var secondPoint = new Point(
            x: actualWidth,
            y: actualHeight * 0.5
        );
        var thirdPoint = new Point(
            x: actualWidth * 0.5,
            y: actualHeight
        );
        var finalPoint = new Point(
            x: 0,
            y: actualHeight * 0.5
        );

        var geometry = new StreamGeometry();
        using StreamGeometryContext context = geometry.Open();

        context.BeginFigure(
            startPoint: startingPoint,
            isFilled: true,
            isClosed: true
        );
        context.LineTo(
            point: secondPoint,
            isStroked: true,
            isSmoothJoin: false
        );
        context.LineTo(
            point: thirdPoint,
            isStroked: true,
            isSmoothJoin: false
        );
        context.LineTo(
            point: finalPoint,
            isStroked: true,
            isSmoothJoin: false
        );

        double offset = StrokeThickness * 0.5;
        geometry.Transform = new TranslateTransform(
            offsetX: offset,
            offsetY: offset
        );



        return geometry;
    }
}
