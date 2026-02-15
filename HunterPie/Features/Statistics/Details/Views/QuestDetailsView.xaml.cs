using HunterPie.Features.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HunterPie.Features.Statistics.Details.Views;

/// <summary>
/// Interaction logic for QuestDetailsView.xaml
/// </summary>
internal partial class QuestDetailsView : UserControl, IView<QuestDetailsViewModel>
{
    private readonly Storyboard _slideInAnimation;
    public QuestDetailsViewModel ViewModel => (QuestDetailsViewModel)DataContext;

    public QuestDetailsView()
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