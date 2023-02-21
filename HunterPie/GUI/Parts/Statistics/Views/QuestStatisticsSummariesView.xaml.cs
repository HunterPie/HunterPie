﻿using HunterPie.GUI.Parts.Statistics.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.GUI.Parts.Statistics.Views;

/// <summary>
/// Interaction logic for QuestStatisticsSummariesView.xaml
/// </summary>
public partial class QuestStatisticsSummariesView : View<QuestStatisticsSummariesViewModel>
{
    public QuestStatisticsSummariesView()
    {
        InitializeComponent();
    }

    protected override void Initialize() => ViewModel.FetchQuests();

    private void OnRequestPageUpdate(object sender, RoutedEventArgs e) => ViewModel.RequestPageUpdate();
}