using HunterPie.UI.Architecture;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace HunterPie.GUI.Parts;

/// <summary>
/// Interaction logic for HeaderBar.xaml
/// </summary>
public partial class HeaderBar : View<HeaderBarViewModel>
{

    public HeaderBar()
    {
        InitializeComponent();
    }

    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        ViewModel.FetchSupporterStatus();
    }

    private void OnCloseButtonClick(object sender, EventArgs e) => ViewModel.CloseApplication();
    private void OnMinimizeButtonClick(object sender, EventArgs e) => ViewModel.MinimizeApplication();
    private void OnLeftMouseDown(object sender, MouseButtonEventArgs e) => ViewModel.DragApplication();
    private void OnNotificationsClick(object sender, EventArgs e) => ViewModel.IsNotificationsToggled = !ViewModel.IsNotificationsToggled;
}
