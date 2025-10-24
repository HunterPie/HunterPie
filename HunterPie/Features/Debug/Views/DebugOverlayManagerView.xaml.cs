using HunterPie.Features.Debug.ViewModels;
using System.Windows;

namespace HunterPie.Features.Debug.Views;
/// <summary>
/// Interaction logic for DebugOverlayManagerView.xaml
/// </summary>
public partial class DebugOverlayManagerView : Window
{
    public DebugOverlayManagerView()
    {
        InitializeComponent();
    }

    private void OnSettingsButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not DebugOverlayManagerViewModel viewModel)
            return;

        viewModel.NavigateToSettings();
    }
}