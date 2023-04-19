using HunterPie.Core.System;
using HunterPie.GUI.Parts.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Statistics.Views;

/// <summary>
/// Interaction logic for QuestStatisticsSummariesView.xaml
/// </summary>
public partial class QuestStatisticsSummariesView : View<QuestStatisticsSummariesViewModel>
{
    private const string PATREON_LINK = "https://www.patreon.com/HunterPie";

    public QuestStatisticsSummariesView()
    {
        InitializeComponent();
    }

    protected override void Initialize() => ViewModel.FetchQuests();

    private void OnRequestPageUpdate(object sender, RoutedEventArgs e) => ViewModel.RequestPageUpdate();

    private void OnSupporterPromptClick(object sender, RoutedEventArgs e) => BrowserService.OpenUrl(PATREON_LINK);

    private void OnSummaryClick(object sender, RoutedEventArgs e)
    {
        if (sender is IView<QuestStatisticsSummaryViewModel> view && view.ViewModel.UploadId is { } uploadId)
            ViewModel.NavigateToHuntDetails(uploadId);
    }
}