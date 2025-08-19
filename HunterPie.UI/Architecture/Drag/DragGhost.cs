using HunterPie.Core.Observability.Logging;
using HunterPie.UI.Architecture.Adorners;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace HunterPie.UI.Architecture.Drag;

public static class DragGhost
{
    private static readonly ILogger Logger = LoggerFactory.Create();

    private const string DRAG_MOUSE_OFFSET_FORMAT = "DragMouseOffset";

    private static DragAdorner _adorner;
    private static AdornerLayer _layer;

    public static void Attach(FrameworkElement scope)
    {
        scope.AllowDrop = true;
        scope.DragEnter += OnDragEnter;
        scope.DragOver += OnDragOver;
        scope.DragLeave += OnDragLeave;
        scope.Drop += OnDrop;
        scope.GiveFeedback += OnGiveFeedback;
    }

    public static void BeginDrag(UIElement dragVisual, IDataObject data, Point mouseOffset)
    {
        // Store the offset once so all drop surfaces can use it
        data.SetData(DRAG_MOUSE_OFFSET_FORMAT, mouseOffset);
        DragDrop.DoDragDrop(dragVisual, data, DragDropEffects.Move | DragDropEffects.Copy);
    }

    private static void OnGiveFeedback(object s, GiveFeedbackEventArgs e)
    {
        // Hide default cursor so the ghost is the “cursor”
        e.UseDefaultCursors = true;
        e.Handled = true;
    }

    private static void OnDragEnter(object sender, DragEventArgs e)
    {
        var host = (UIElement)sender;
        _layer ??= AdornerLayer.GetAdornerLayer(host);

        if (_layer == null)
            return;

        if (_adorner == null && e.Data.GetDataPresent(typeof(UIElement)))
        {
            var visual = (UIElement)e.Data.GetData(typeof(UIElement));
            _adorner = new DragAdorner(host, visual);

            if (e.Data.GetDataPresent(DRAG_MOUSE_OFFSET_FORMAT))
                _adorner.MouseOffset = (Point)e.Data.GetData(DRAG_MOUSE_OFFSET_FORMAT);

            _layer.Add(_adorner);
        }

        UpdatePosition(host, e);
    }

    private static void OnDragOver(object sender, DragEventArgs e) => UpdatePosition((UIElement)sender, e);

    private static void UpdatePosition(UIElement host, DragEventArgs e)
    {
        if (_adorner == null)
            return;

        Point p = e.GetPosition(host);
        _adorner.Position = p;
        _adorner.InvalidateVisual();
    }

    private static void OnDragLeave(object sender, DragEventArgs e) => Remove();

    private static void OnDrop(object sender, DragEventArgs e) => Remove();

    private static void Remove()
    {
        if (_adorner == null || _layer == null)
            return;

        _layer.Remove(_adorner);
        _adorner = null;
        _layer = null;
        Mouse.SetCursor(Cursors.Arrow);
    }
}