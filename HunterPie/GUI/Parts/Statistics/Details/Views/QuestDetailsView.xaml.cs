using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace HunterPie.GUI.Parts.Statistics.Details.Views;

/// <summary>
/// Interaction logic for QuestDetailsView.xaml
/// </summary>
public partial class QuestDetailsView : UserControl, IView<QuestDetailsViewModel>
{
    private readonly Storyboard _slideInAnimation;
    public QuestDetailsViewModel ViewModel => DataContext as QuestDetailsViewModel;

    public QuestDetailsView()
    {
        InitializeComponent();
        _slideInAnimation = FindResource("SlideInAnimation") as Storyboard;
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPreviousPage();

    private void OnMonsterPanelViewModelChanged(object sender, DependencyPropertyChangedEventArgs _)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void OnMonsterPanelLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement element)
            AnimatePanel(element);
    }

    private void AnimatePanel(FrameworkElement element) => _slideInAnimation.Begin(element);
}