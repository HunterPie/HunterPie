using HunterPie.Core.Extensions;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.UI.Controls.Display;
/// <summary>
/// Interaction logic for Carousel.xaml
/// </summary>
public partial class Carousel : UserControl
{
    public IList ItemsSource
    {
        get => (IList)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    public static readonly DependencyProperty ItemsSourceProperty =
        DependencyProperty.Register(nameof(ItemsSource), typeof(IList), typeof(Carousel), new PropertyMetadata(OnItemsSourceChange));

    public DataTemplateSelector ItemTemplateSelector
    {
        get => (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }
    public static readonly DependencyProperty ItemTemplateSelectorProperty =
        DependencyProperty.Register(nameof(ItemTemplateSelector), typeof(DataTemplateSelector), typeof(Carousel));

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    public static readonly DependencyProperty ItemTemplateProperty =
        DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(Carousel));

    public object Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }
    public static readonly DependencyProperty ItemProperty =
        DependencyProperty.Register(nameof(Item), typeof(object), typeof(object));

    public int SelectedItem
    {
        get => (int)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register(nameof(SelectedItem), typeof(int), typeof(Carousel), new PropertyMetadata(OnSelectedItemChange));

    public Carousel()
    {
        InitializeComponent();
    }

    private void OnPreviousClick(object sender, RoutedEventArgs e)
    {
        int nextSlide = SelectedItem - 1;

        SelectedItem = nextSlide >= 0 ? nextSlide : ItemsSource.Count();
    }

    private void OnNextClick(object sender, RoutedEventArgs e)
    {
        int nextSlide = SelectedItem + 1;

        SelectedItem = nextSlide >= ItemsSource.Count ? 0 : nextSlide;
    }

    private static void OnItemsSourceChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;

        carousel.Item = carousel.ItemsSource[0];
    }

    private static void OnSelectedItemChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Carousel carousel)
            return;

        carousel.Item = carousel.ItemsSource[carousel.SelectedItem];
    }
}
