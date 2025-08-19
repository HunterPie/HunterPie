using HunterPie.UI.Architecture.Drag;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.Features.Theme.Views;
/// <summary>
/// Interaction logic for InstalledThemeHomeTabView.xaml
/// </summary>
public partial class InstalledThemeHomeTabView : UserControl
{
    private Point _mousePosition;
    private FrameworkElement? _draggedElement;

    public InstalledThemeHomeTabView()
    {
        InitializeComponent();
    }

    private void OnLeftMouseUp(object sender, MouseButtonEventArgs e)
    {
        _draggedElement = null;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        _draggedElement = null;
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_draggedElement is not { } element)
            return;

        var data = new DataObject();
        data.SetData(typeof(UIElement), element);

        DragGhost.BeginDrag(element, data, _mousePosition);

        e.Handled = true;
    }

    private void OnElementsPanelLoaded(object sender, RoutedEventArgs e)
    {
        DragGhost.Attach((FrameworkElement)sender);
    }

    private void OnElementBeginDrag(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement element)
            return;

        _draggedElement = element;
        _mousePosition = Mouse.GetPosition(_draggedElement);
    }
}
