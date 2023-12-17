using HunterPie.Core.Client;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace HunterPie.UI.Main.Views;
/// <summary>
/// Interaction logic for MainView.xaml
/// </summary>
public partial class MainView : Window
{
    private static readonly DoubleAnimation ScaleDownAnimation =
        new(1.5, 1, TimeSpan.FromMilliseconds(200)) { EasingFunction = new QuarticEase() };

    private static readonly DoubleAnimation FadeInAnimation = new(0, 1, TimeSpan.FromMilliseconds(500))
    {
        EasingFunction = new SineEase()
    };

    public MainView()
    {
        InitializeComponent();
    }

    private void OnDragStart(object sender, RoutedEventArgs e) => DragMove();

    private void OnCloseClick(object sender, RoutedEventArgs e) => Close();

    private void OnMinimizeClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;

        if (ClientConfig.Config.Client.MinimizeToSystemTray)
            Hide();
    }

    private void OnNavigationTargetUpdated(object? sender, DataTransferEventArgs e)
    {
        PART_NavigationHost.BeginAnimation(OpacityProperty, FadeInAnimation);
        PART_NavigationHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, ScaleDownAnimation);
        PART_NavigationHost.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, ScaleDownAnimation);
    }
}
