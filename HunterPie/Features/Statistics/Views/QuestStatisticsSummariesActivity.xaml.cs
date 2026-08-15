using HunterPie.Core.System;
using HunterPie.Features.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Views;
using HunterPie.UI.Controls.Paginating.Events;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.Features.Statistics.Views;

/// <summary>
/// Interaction logic for QuestStatisticsSummariesActivity.xaml
/// </summary>
[View<QuestStatisticsSummariesViewModel>]
internal partial class QuestStatisticsSummariesActivity : Activity
{
    private const string PATREON_LINK = "https://www.patreon.com/HunterPie";

    public QuestStatisticsSummariesActivity()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
        => await ViewModel.FetchQuests();

    private void OnSupporterPromptClick(object sender, RoutedEventArgs e) => BrowserService.OpenUrl(PATREON_LINK);

    private void OnSummaryClick(object sender, RoutedEventArgs e)
    {
        if (sender is IView<QuestStatisticsSummaryViewModel> view && view.ViewModel.UploadId is { } uploadId)
            ViewModel.NavigateToHuntDetails(uploadId);
    }

    private void OnPageClick(object sender, PaginationEventArgs e)
    {
        ViewModel.CurrentPage = e.Page;
    }

    private async void OnLimitSelectionChange(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is null)
            return;

        await ViewModel.FetchOrRefresh();
    }
}