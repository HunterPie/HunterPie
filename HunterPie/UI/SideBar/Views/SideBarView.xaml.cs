using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using ResourceService = HunterPie.UI.Assets.Application.Resources;

namespace HunterPie.UI.SideBar.Views;

/// <summary>
/// Interaction logic for SideBarView.xaml
/// </summary>
public partial class SideBarView : UserControl
{
    private static readonly double SidebarWidth = ResourceService.Get<double>("HUNTERPIE_SIDEBAR_WIDTH");
    private static readonly Duration AnimationDuration = new Duration(TimeSpan.FromMilliseconds(300));
    private static readonly DoubleAnimation OpenSideBarAnimation =
        new(SidebarWidth, AnimationDuration)
        {
            EasingFunction = new QuadraticEase()
        };

    private static readonly DoubleAnimation CloseSideBarAnimation =
        new(50.0, AnimationDuration)
        {
            EasingFunction = new QuadraticEase()
        };

    public SideBarView()
    {
        InitializeComponent();
    }

    private void OnPreviewMouseMove(object sender, MouseEventArgs e)
    {
        Point sideBarPosition = e.GetPosition(PART_SideBar);
        HitTestResult? sideBarHitTestResult = VisualTreeHelper.HitTest(PART_SideBar, sideBarPosition);
        bool isMouseOverSideBar = sideBarHitTestResult is not null && PART_SideBar.IsHitTestVisible;

        Point hitTestPosition = e.GetPosition(PART_HitTest);
        HitTestResult? maskHitTestResult = VisualTreeHelper.HitTest(PART_HitTest, hitTestPosition);
        bool isMouseOverMask = maskHitTestResult is not null;
        bool isMouseOver = isMouseOverMask || isMouseOverSideBar;
        PART_SideBar.IsHitTestVisible = isMouseOver;

        AnimateMask(isMouseOver);
    }

    private void OnMouseLeave(object sender, MouseEventArgs e)
    {
        PART_SideBar.IsHitTestVisible = false;
        AnimateMask(false);
    }

    private void AnimateMask(bool isMouseOver)
    {
        DoubleAnimation animation = isMouseOver ? OpenSideBarAnimation : CloseSideBarAnimation;
        PART_HitTest.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation, HandoffBehavior.SnapshotAndReplace);
    }
}