using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.UI.Settings.ViewModels.Internal;
using System.Windows;
using System.Windows.Controls;
using Button = HunterPie.UI.Controls.Buttons.Button;

namespace HunterPie.UI.Settings.Views;
/// <summary>
/// Interaction logic for AbnormalityTrayConfigurationPropertyView.xaml
/// </summary>
public partial class AbnormalityTrayConfigurationPropertyView : UserControl
{
    public AbnormalityTrayConfigurationPropertyView()
    {
        InitializeComponent();
    }

    private void OnCreateNewTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        vm.CreateNewTray();
    }

    private void OnDeleteTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        if (sender is not Button { Key: AbnormalityWidgetConfig tray })
            return;

        vm.DeleteTray(tray);
    }

    private void OnConfigureTrayButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataContext is not AbnormalityTrayPropertyViewModel vm)
            return;

        if (sender is not Button { Key: AbnormalityWidgetConfig tray })
            return;

        vm.ConfigureTray(tray);
    }
}