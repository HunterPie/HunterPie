using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Media.Animation;

namespace HunterPie.Features.Statistics.Details.Views;

/// <summary>
/// Interaction logic for QuestDetailsActivity.xaml
/// </summary>
internal partial class QuestDetailsActivity : Activity
{
    private readonly Storyboard _slideInAnimation;
    private QuestDetailsViewModel ViewModel => (QuestDetailsViewModel)DataContext;

    public QuestDetailsActivity()
    {
        InitializeComponent();
        _slideInAnimation = (Storyboard)FindResource("SlideInAnimation");
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPreviousPage();

    // Do not remove this, it is required to trigger the animation when the view model changes
    private void OnMonsterPanelViewModelChanged(object sender, DependencyPropertyChangedEventArgs _) =>
        SetupView(sender);

    private void OnMonsterPanelLoaded(object sender, RoutedEventArgs _) =>
        SetupView(sender);

    private void SetupView(object obj)
    {
        if (obj is not MonsterDetailsView { DataContext: MonsterDetailsViewModel } view)
            return;

        view.InitializeView();
        AnimatePanel(view);
    }

    private void AnimatePanel(FrameworkElement element) => _slideInAnimation.Begin(element);
}