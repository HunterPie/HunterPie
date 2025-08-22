using HunterPie.Core.Extensions;
using HunterPie.Features.Theme.ViewModels;
using HunterPie.UI.Architecture.Adorners;
using HunterPie.UI.Architecture.Tree;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace HunterPie.Features.Theme.Views;

/// <summary>
/// Interaction logic for InstalledThemeHomeTabView.xaml
/// </summary>
public partial class InstalledThemeHomeTabView : UserControl
{
    private InstalledThemeViewModel? _lastHoveredViewModel;
    private DragAdorner<InstalledThemeView>? _draggedElement;
    private AdornerLayer? _adornerLayer;

    private InstalledThemeHomeTabViewModel ViewModel => (InstalledThemeHomeTabViewModel)DataContext;

    public InstalledThemeHomeTabView()
    {
        InitializeComponent();
    }

    private void OnLeftMouseUp(object sender, MouseButtonEventArgs e) => EndDragging();

    private void OnMouseLeave(object sender, MouseEventArgs e) => EndDragging();

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        if (_draggedElement is not { } element)
            return;

        e.Handled = true;

        Point currentPosition = e.GetPosition(this);

        element.Position = currentPosition;

        HitTestResult result = VisualTreeHelper.HitTest(this, currentPosition);

        if (result.VisualHit.TryFindParent<InstalledThemeView>() is not
            { DataContext: InstalledThemeViewModel hoveredViewModel })
        {
            _lastHoveredViewModel?.Let(it => it.IsDraggingOver = false);
            return;
        }

        if (_lastHoveredViewModel != hoveredViewModel)
            _lastHoveredViewModel?.Let(it => it.IsDraggingOver = false);

        hoveredViewModel.IsDraggingOver = true;
        _lastHoveredViewModel = hoveredViewModel;
    }

    private void OnElementsPanelLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement container)
            return;

        _adornerLayer = AdornerLayer.GetAdornerLayer(container);
    }

    private void OnElementBeginDrag(object sender, RoutedEventArgs e)
    {
        if (sender is not InstalledThemeView element)
            return;

        _draggedElement = new DragAdorner<InstalledThemeView>(this, element)
        {
            MouseOffset = Mouse.GetPosition(element),
            Position = Mouse.GetPosition(this)
        };

        _adornerLayer?.Add(_draggedElement);
    }

    private void EndDragging()
    {
        if (_draggedElement is not { ConcreteElement.DataContext: InstalledThemeViewModel sourceViewModel })
            return;

        if (_lastHoveredViewModel is { } targetViewModel)
        {
            int oldIndex = ViewModel.Themes.IndexOf(sourceViewModel);
            int newIndex = ViewModel.Themes.IndexOf(targetViewModel);

            ViewModel.Themes.Move(oldIndex, newIndex);
            targetViewModel.IsDraggingOver = false;
        }

        _adornerLayer?.Remove(_draggedElement);
        _draggedElement = null;
        _lastHoveredViewModel = null;
    }
}
