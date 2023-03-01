using HunterPie.GUI.Parts.Statistics.Details.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Statistics.Details.Views;

/// <summary>
/// Interaction logic for QuestDetailsView.xaml
/// </summary>
public partial class QuestDetailsView : UserControl, IView<QuestDetailsViewModel>
{

    public QuestDetailsViewModel ViewModel => DataContext as QuestDetailsViewModel;

    public QuestDetailsView()
    {
        InitializeComponent();
    }

    private void OnBackButtonClick(object sender, RoutedEventArgs e) => ViewModel.NavigateToPreviousPage();

}