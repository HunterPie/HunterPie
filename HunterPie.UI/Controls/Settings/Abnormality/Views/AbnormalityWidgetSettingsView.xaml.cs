using HunterPie.UI.Controls.Settings.Abnormality.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Abnormality.Views;
/// <summary>
/// Interaction logic for AbnormalityWidgetSettingsView.xaml
/// </summary>
public partial class AbnormalityWidgetSettingsView : UserControl
{
    public AbnormalityWidgetSettingsView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityWidgetSettingsViewModel vm)
            return;

        vm.ExitScreen();
    }
}
