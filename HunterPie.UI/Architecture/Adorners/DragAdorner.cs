using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Adorners;

public sealed class DragAdorner : Adorner
{
    private readonly VisualBrush _brush;
    private readonly Size _size;
    private Point _offset;
    public Point MouseOffset
    {
        get => _offset;
        set
        {
            _offset = value;
            InvalidateVisual();
        }
    }
    public Point Position { get; set; }

    public DragAdorner(UIElement adornerLayerOwner, UIElement visualToDisplay) : base(adornerLayerOwner)
    {
        IsHitTestVisible = false;
        _brush = new VisualBrush(visualToDisplay)
        {
            Opacity = 0.7,
            Stretch = Stretch.None,
            AlignmentX = AlignmentX.Left,
            AlignmentY = AlignmentY.Top
        };
        visualToDisplay.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        _size = visualToDisplay.RenderSize;
    }

    protected override void OnRender(DrawingContext dc)
    {
        var topLeft = new Point(Position.X - MouseOffset.X, Position.Y - MouseOffset.Y);
        dc.DrawRectangle(_brush, null, new Rect(topLeft, _size));
    }
}