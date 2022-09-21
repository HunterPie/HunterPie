using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Domain.Features;
using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar.ViewModels;
using HunterPie.UI.Architecture.Extensions;
using System;
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
    private bool _isMouseDown;
    private readonly Storyboard _selectSlideAnimation;

    public double ItemsHeight
    {
        get => (double)GetValue(ItemsHeightProperty);
        set => SetValue(ItemsHeightProperty, value);
    }
    public static readonly DependencyProperty ItemsHeightProperty =
        DependencyProperty.Register("ItemsHeight", typeof(double), typeof(SideBarContainer), new PropertyMetadata(20.0));

    public Thickness SelectedButton
    {
        get => (Thickness)GetValue(SelectedButtonProperty);
        set => SetValue(SelectedButtonProperty, value);
    }
    public static readonly DependencyProperty SelectedButtonProperty =
        DependencyProperty.Register("SelectedButton", typeof(Thickness), typeof(SideBarContainer));

    public SideBarContainer()
    {
        InitializeComponent();
        _selectSlideAnimation = this.FindResource<Storyboard>("PART_SelectionAnimation");
        DataContext = this;
    }

    protected override void OnInitialized(EventArgs e)
    {
        if (!FeatureFlagManager.IsEnabled(FeatureFlags.FEATURE_USER_ACCOUNT))
            PART_UserAccount.Visibility = Visibility.Collapsed;

        base.OnInitialized(e);
    }

    public static void SetMenu(ISideBar menu) => Add(menu.Menu);

    public static void Add(params ISideBarElement[] elements)
    {
        foreach (ISideBarElement element in elements)
            _elements.Add(element);
    }

    private void OnLeftMouseButtonDown(object sender, MouseButtonEventArgs e) => _isMouseDown = true;

    private void OnLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
        // Was a click!
        if (_isMouseDown && IsMouseInside)
            OnClick(e.GetPosition(this));

        _isMouseDown = false;
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = true;
        PART_ButtonsContainer.IsHitTestVisible = true;
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        IsMouseInside.Value = false;
        _isMouseDown = false;
        PART_ButtonsContainer.IsHitTestVisible = false;
    }

    private void OnClick(Point mousePosition)
    {
        int idx = (int)Math.Abs(mousePosition.Y / ItemsHeight);

        if (idx >= Elements.Count)
            return;

        ISideBarElement element = Elements[idx];

        if (!element.IsActivable || !element.IsEnabled)
            return;

        ((ThicknessAnimation)_selectSlideAnimation.Children[0]).To = new Thickness(0, idx * ItemsHeight, 0, 0);

        PART_Selection.BeginStoryboard(_selectSlideAnimation);
    }
}
