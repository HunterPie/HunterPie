using HunterPie.UI.Overlay.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Overlay.Views;

/// <summary>
/// Interaction logic for WidgetHeader.xaml
/// </summary>
public partial class WidgetHeader : UserControl
{
    public WidgetView Owner { get; private set; }

    public WidgetHeader()
    {
        InitializeComponent();
    }

    private void OnCloseButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not WidgetContext vm)
            return;

        vm.ViewModel.Settings.Initialize.Value = false;
    }

    private void OnHideButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not WidgetContext vm)
            return;

        vm.ViewModel.Settings.Enabled.Value = !vm.ViewModel.Settings.Enabled.Value;
    }

    private void OnLoaded(object sender, RoutedEventArgs e) => Owner = (WidgetView)Window.GetWindow(this);

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        Owner.DragMove();
    }
}