using HunterPie.UI.Architecture.Brushes;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace HunterPie.UI.Architecture.Adorners;

public sealed class DragAdorner<T> : Adorner where T : UIElement
{
    private readonly Brush _brush;
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

    private Point _position;
    public Point Position
    {
        get => _position;
        set
        {
            _position = value;
            InvalidateVisual();
        }
    }

    public T ConcreteElement { get; }

    public DragAdorner(UIElement adornerLayerOwner, T visualToDisplay) : base(adornerLayerOwner)
    {
        IsHitTestVisible = false;
        _brush = SnapshotBrush.From(visualToDisplay);
        visualToDisplay.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
        _size = visualToDisplay.RenderSize;
        ConcreteElement = visualToDisplay;
    }

    protected override void OnRender(DrawingContext dc)
    {
        var topLeft = new Point(Position.X - MouseOffset.X, Position.Y - MouseOffset.Y);
        dc.DrawRectangle(_brush, null, new Rect(topLeft, _size));
    }
}