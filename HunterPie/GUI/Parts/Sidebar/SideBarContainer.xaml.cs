using HunterPie.Core.Architecture;
using HunterPie.GUI.Parts.Sidebar.Service;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using HunterPie.UI.Architecture.Extensions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HunterPie.GUI.Parts.Sidebar;

/// <summary>
/// Interaction logic for SideBarContainer.xaml
/// </summary>
public partial class SideBarContainer : UserControl
{
    // TODO: Make a ViewModel for this
    private static readonly ObservableCollection<ISideBarElement> _elements = new();

    public ObservableCollection<ISideBarElement> Elements => _elements;

    public Observable<bool> IsMouseInside { get; } = false;
    private readonly Storyboard _selectSlideAnimation;

    public double ItemsHeight
    {
        get => (double)GetValue(ItemsHeightProperty);
        set => SetValue(ItemsHeightProperty, value);
    }
    public static readonly DependencyProperty ItemsHeightProperty =
        DependencyProperty.Register("ItemsHeight", typeof(double), typeof(SideBarContainer), new PropertyMetadata(40.0));

    public Thickness SelectedButton
    {
        get => (Thickness)GetValue(SelectedButtonProperty);
        set => SetValue(SelectedButtonProperty, value);
    }
    public static readonly DependencyProperty SelectedButtonProperty =
        DependencyProperty.Register("SelectedButton", typeof(Thickness), typeof(SideBarContainer));

    public SideBarContainer()
    {
        HookEvents();

        InitializeComponent();

        _selectSlideAnimation = this.FindResource<Storyboard>("PART_SelectionAnimation");
        DataContext = this;

        if (SideBarService.CurrentlySelected is not null)
            NavigateTo(SideBarService.CurrentlySelected);
    }

    private void HookEvents() => SideBarService.NavigateToElement += NavigateTo;

    public static void SetMenu(ISideBarElement[] menu) => Add(menu);

    public static void Add(params ISideBarElement[] elements)
    {
        foreach (ISideBarElement element in elements)
            _elements.Add(element);
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = true;
        PART_ButtonsContainer.IsHitTestVisible = true;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = false;
        PART_ButtonsContainer.IsHitTestVisible = false;
    }

    private void NavigateTo(ISideBarElement element)
    {
        if (!element.IsActivable || !element.IsEnabled)
            return;

        int idx = Elements.IndexOf(element);

        ((ThicknessAnimation)_selectSlideAnimation.Children[0]).To = new Thickness(0, idx * ItemsHeight, 0, 0);

        PART_Selection.BeginStoryboard(_selectSlideAnimation);
    }
}
