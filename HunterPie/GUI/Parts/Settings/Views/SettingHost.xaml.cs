using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.UI.Controls.TextBox.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HunterPie.GUI.Parts.Settings.Views;

/// <summary>
/// Interaction logic for SettingHost.xaml
/// </summary>
public partial class SettingHost : UserControl
{
    private readonly Storyboard _slideInAnimation;
    public SettingHostViewModel ViewModel => (SettingHostViewModel)DataContext;

    public SettingHost()
    {
        InitializeComponent();
        _slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
    }

    private void OnRealTimeSearch(object sender, SearchTextChangedEventArgs e) => ViewModel.SearchSetting(e.Text);
    private void OnLoaded(object sender, RoutedEventArgs e) => ViewModel.FetchVersion();
    private void OnUnloaded(object sender, RoutedEventArgs e) => ViewModel.UnhookEvents();
    private void OnExecuteUpdateClick(object sender, RoutedEventArgs e) => ViewModel.ExecuteRestart();

    private void OnPanelLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void OnPanelDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void AnimatePanel(FrameworkElement element) => _slideInAnimation.Begin(element);
}
