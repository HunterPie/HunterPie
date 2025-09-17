using HunterPie.UI.Overlay.Widgets.Damage.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Damage.View;

/// <summary>
/// Interaction logic for MeterView.xaml
/// </summary>
public partial class MeterView
{
    private MeterViewModel ViewModel => (MeterViewModel)DataContext;

    public MeterView()
    {
        InitializeComponent();
    }

    private void OnPlayerHighlightToggle(object sender, EventArgs e) => ViewModel.ToggleHighlight();

    private void OnPlayerBlurToggle(object sender, EventArgs e) => ViewModel.ToggleBlur();
}