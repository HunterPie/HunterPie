using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Features.Account;
using HunterPie.Features.Account.Event;
using HunterPie.GUI.Parts.Account.ViewModels;
using HunterPie.UI.Architecture;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace HunterPie.GUI.Parts.Account.Views;

/// <summary>
/// Interaction logic for AccountSignFlowView.xaml
/// </summary>
public partial class AccountSignFlowView : View<AccountSignFlowViewModel>, IEventDispatcher, IDisposable
{
    private Storyboard SlideOutAnimation;
    public event EventHandler<EventArgs> OnFormClose;

    public AccountSignFlowView()
    {
        HookEvents();

        InitializeComponent();

    }

    public override void EndInit()
    {
        base.EndInit();
        SlideOutAnimation = FindResource("SB_SLIDE_OUT") as Storyboard;
    }

    private void CloseForm()
    {
        this.Dispatch(OnFormClose);
        Dispose();
    }

    private void OnCloseClick(object sender, RoutedEventArgs e) => AnimateSlideOut();

    public void Dispose()
    {
        AccountManager.OnSignIn -= OnAccountSignIn;
        ViewModel.PropertyChanged -= OnPropertyChanged;
    }

    private void HookEvents()
    {
        AccountManager.OnSignIn += OnAccountSignIn;
        ViewModel.PropertyChanged += OnPropertyChanged;
    }

    private void OnAccountSignIn(object sender, AccountLoginEventArgs e) => AnimateSlideOut();

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(AccountSignFlowViewModel.SelectedIndex):
                AnimateSlide(ViewModel.SelectedIndex);
                break;
            default:
                break;
        }
    }

    private void AnimateSlide(int index)
    {
        Thickness[] positions = { new Thickness(12, 12, 0, 10), new Thickness(198, 12, 0, 10) };
        ThicknessAnimation animation = new(positions[index], TimeSpan.FromMilliseconds(200), FillBehavior.HoldEnd)
        {
            EasingFunction = new QuadraticEase(),
            DecelerationRatio = 1,
        };

        PART_SelectedSignFlowHighlight.BeginAnimation(FrameworkElement.MarginProperty, animation);
    }

    private void AnimateSlideOut() => SlideOutAnimation.Begin(PART_Border);

    private void OnSlideOutCompleted(object sender, EventArgs e) => CloseForm();
}
