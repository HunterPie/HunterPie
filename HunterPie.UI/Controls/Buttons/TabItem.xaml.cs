using HunterPie.UI.Architecture.Tree;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using NativeTabItem = System.Windows.Controls.TabItem;

namespace HunterPie.UI.Controls.Buttons;

/// <summary>
/// Interaction logic for TabItem.xaml
/// </summary>
public partial class TabItem : UserControl
{
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public static readonly DependencyProperty TitleProperty =
        DependencyProperty.Register(nameof(Title), typeof(string), typeof(TabItem), new PropertyMetadata("Header"));

    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
    public static readonly DependencyProperty DescriptionProperty =
        DependencyProperty.Register(nameof(Description), typeof(string), typeof(TabItem), new PropertyMetadata("Header tooltip"));

    public ImageSource Icon
    {
        get => (ImageSource)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }
    public static readonly DependencyProperty IconProperty =
        DependencyProperty.Register(nameof(Icon), typeof(ImageSource), typeof(TabItem));

    public TabItem()
    {
        InitializeComponent();
    }

    private void OnClick(object sender, RoutedEventArgs e)
    {
        this.TryFindParent<NativeTabItem>()?
            .SetCurrentValue(NativeTabItem.IsSelectedProperty, true);
    }
}