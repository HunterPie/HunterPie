using HunterPie.GUI.Parts.Backup.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace HunterPie.GUI.Parts.Backup.Views;
/// <summary>
/// Interaction logic for BackupElementView.xaml
/// </summary>
public partial class BackupElementView : UserControl
{
    private BackupElementViewModel ViewModel => DataContext as BackupElementViewModel;

    public BackupElementView()
    {
        InitializeComponent();
    }

    private async void OnDownloadClick(object sender, RoutedEventArgs e) => await ViewModel.Download();
    private void OnOpenFolderClick(object sender, RoutedEventArgs e) => ViewModel.OpenBackupFolder();
    private async void OnDeleteClick(object sender, RoutedEventArgs e) => await ViewModel.Delete();
}
