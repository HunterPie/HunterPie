﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HunterPie.UI.Controls.Progress;

/// <summary>
/// Interaction logic for Gauge.xaml
/// </summary>
public partial class Gauge : UserControl
{
    public ObservableCollection<bool> MarkerCollection { get; } = new();

    public double Current
    {
        get => (double)GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    public static readonly DependencyProperty CurrentProperty =
        DependencyProperty.Register(nameof(Current), typeof(double), typeof(Gauge), new PropertyMetadata(0.0));

    public double Max
    {
        get => (double)GetValue(MaxProperty);
        set => SetValue(MaxProperty, value);
    }

    public static readonly DependencyProperty MaxProperty =
        DependencyProperty.Register(nameof(Max), typeof(double), typeof(Gauge), new PropertyMetadata(0.0));

    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }
    public static new readonly DependencyProperty ForegroundProperty =
        DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(Gauge));

    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    public static new readonly DependencyProperty BackgroundProperty =
        DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(Gauge));

    public new Brush BorderBrush
    {
        get => (Brush)GetValue(BorderBrushProperty);
        set => SetValue(BorderBrushProperty, value);
    }

    public static new readonly DependencyProperty BorderBrushProperty =
        DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(Gauge));

    public Orientation Orientation
    {
        get => (Orientation)GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly DependencyProperty OrientationProperty =
        DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(Gauge), new PropertyMetadata(Orientation.Horizontal));

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(Gauge), new PropertyMetadata(5.0));

    public int Markers
    {
        get => (int)GetValue(MarkersProperty);
        set => SetValue(MarkersProperty, value);
    }

    public static readonly DependencyProperty MarkersProperty =
        DependencyProperty.Register(nameof(Markers), typeof(int), typeof(Gauge), new PropertyMetadata(OnMarkersChange));

    public Gauge()
    {
        InitializeComponent();
    }


    private static void OnMarkersChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Gauge gauge || e.NewValue is not int markers)
            return;

        gauge.MarkerCollection.Clear();

        for (int i = 0; i < markers; i++)
        {
            bool isMarkerVisible = gauge.Orientation == Orientation.Horizontal
                ? i > 0
                : i < (markers - 1);

            gauge.MarkerCollection.Add(isMarkerVisible);
        }

    }
}