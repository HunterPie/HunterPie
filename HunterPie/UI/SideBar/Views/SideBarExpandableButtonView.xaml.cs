using HunterPie.UI.Architecture.Animation;
using HunterPie.UI.Architecture.Utils;
using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Controls.Buttons;
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace HunterPie.UI.SideBar.Views;
/// <summary>
/// Interaction logic for SideBarExpandableButtonView.xaml
/// </summary>
[View<NavigationHandler.Group>]
public partial class SideBarExpandableButtonView
{
    private readonly CornerRadiusAnimation _cornerRadiusAnimation = new CornerRadiusAnimation
    {
        TopRight = 0,
        BottomRight = 0,
        Duration = TimeSpan.FromMilliseconds(250)
    };

    private readonly CornerRadiusAnimation _restoreCornerRadiusAnimation = new CornerRadiusAnimation
    {
        TopRight = 0,
        BottomRight = 0,
        Duration = TimeSpan.FromMilliseconds(250)
    };

    public bool IsFixed { get => (bool)GetValue(IsFixedProperty); set => SetValue(IsFixedProperty, value); }

    // Using a DependencyProperty as the backing store for IsFixed.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFixedProperty =
        DependencyProperty.Register(nameof(IsFixed), typeof(bool), typeof(SideBarExpandableButtonView), new PropertyMetadata(false));


    public SideBarExpandableButtonView()
    {
        InitializeComponent();
        _cornerRadiusAnimation.Freeze();
        PART_Popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback(GetSmartPopupPlacement);
    }

    private void OnMouseEnter(object sender, MouseEventArgs e)
    {
        PART_Popup.IsOpen = true;

        PART_Button.BeginAnimation(
            dp: Button.CornerRadiusProperty,
            animation: _cornerRadiusAnimation,
            handoffBehavior: HandoffBehavior.SnapshotAndReplace
        );
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        bool isWithinBoundsOfPopup = PART_Popup.Child switch
        {
            FrameworkElement popup => IsWithinBoundsOf(popup, e),
            _ => false
        };

        if (!IsWithinBoundsOf(this, e) && !isWithinBoundsOfPopup)
        {
            PART_Popup.IsOpen = false;
            PART_Button.BeginAnimation(
                dp: Button.CornerRadiusProperty,
                animation: _restoreCornerRadiusAnimation,
                handoffBehavior: HandoffBehavior.SnapshotAndReplace
            );
            return;
        }


    }

    private CustomPopupPlacement[] GetSmartPopupPlacement(Size popupSize, Size targetSize, Point offset)
    {
        double targetHeight = targetSize.Height - 8;
        double x = targetSize.Width;

        var bottomAlignedPoint = new Point(x, (popupSize.Height - (targetHeight / 2)) * -1);

        var topAlignedPoint = new Point(x, 0);

        return
        [
            new CustomPopupPlacement(topAlignedPoint, PopupPrimaryAxis.Vertical),
            new CustomPopupPlacement(bottomAlignedPoint, PopupPrimaryAxis.Vertical)
        ];
    }

    private void OnButtonLoaded(object sender, RoutedEventArgs e)
    {
        _restoreCornerRadiusAnimation.TopRight = PART_Button.CornerRadius.TopRight;
        _restoreCornerRadiusAnimation.BottomRight = PART_Button.CornerRadius.BottomRight;
        _restoreCornerRadiusAnimation.Freeze();
    }

    private static bool IsWithinBoundsOf(FrameworkElement component, MouseEventArgs e)
    {
        Point point = e.GetPosition(component);

        return point.IsWithinBounds(component);
    }
}