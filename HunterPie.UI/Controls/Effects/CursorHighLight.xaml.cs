﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HunterPie.UI.Controls.Effects;

/// <summary>
/// Interaction logic for CursorHighLight.xaml
/// </summary>
public partial class CursorHighLight : Grid
{
    public static readonly DependencyProperty ChildrenProperty =
        DependencyProperty.Register(
            nameof(Children),
            typeof(object),
            typeof(CursorHighLight),
            new PropertyMetadata(null));

    public new object Children
    {
        get => GetValue(ChildrenProperty);
        set => SetValue(ChildrenProperty, value);
    }

    public CursorHighLight()
    {
        InitializeComponent();
    }

    private void OnMouseMove(object sender, MouseEventArgs e)
    {
        base.OnMouseMove(e);

        if (!IsMouseOver)
            return;

        Point mousePosition = e.GetPosition(this);

        double left = mousePosition.X - (PART_Highlight.ActualWidth / 2);
        double top = mousePosition.Y - (PART_Highlight.ActualHeight / 2);

        PART_Highlight.Margin = new Thickness(left, top, 0, 0);
    }
}