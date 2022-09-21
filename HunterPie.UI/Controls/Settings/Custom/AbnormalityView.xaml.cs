using HunterPie.UI.Controls.Settings.Custom.Abnormality;
using System;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Settings.Custom;

/// <summary>
/// Interaction logic for AbnormalityView.xaml
/// </summary>
public partial class AbnormalityView : UserControl
{
    public AbnormalityView()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, EventArgs e)
    {
        if (DataContext is AbnormalityViewModel vm)
        {
            vm.IsEnabled = !vm.IsEnabled;
        }
    }
}
