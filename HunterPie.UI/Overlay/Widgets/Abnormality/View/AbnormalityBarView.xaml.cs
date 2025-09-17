using HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;
using System;
using System.Windows;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.View;

/// <summary>
/// Interaction logic for AbnormalityBarView.xaml
/// </summary>
public partial class AbnormalityBarView
{
    public AbnormalityBarView()
    {
        InitializeComponent();

    }

    private int _frameCounter = 0;
    private void OnRender(object sender, EventArgs e)
    {
        if (DataContext is not AbnormalityBarViewModel vm)
            return;

        // Sort abnormalities every 60 frames
        if (_frameCounter >= 60)
        {
            vm.SortAbnormalities();
            _frameCounter = 0;
        }

        _frameCounter++;
    }

    private void OnLoad(object sender, RoutedEventArgs e) => CompositionTarget.Rendering += OnRender;

    private void OnUnload(object sender, RoutedEventArgs e) => CompositionTarget.Rendering -= OnRender;
}