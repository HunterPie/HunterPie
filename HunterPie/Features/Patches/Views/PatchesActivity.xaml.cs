using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Architecture;
using System.Windows;

namespace HunterPie.Features.Patches.Views;
/// <summary>
/// Interaction logic for PatchesActivity.xaml
/// </summary>
internal partial class PatchesActivity : Activity
{
    private PatchesViewModel ViewModel => (PatchesViewModel)DataContext;

    public PatchesActivity()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object _, RoutedEventArgs __)
    {
        await ViewModel.FetchPatchesAsync();
    }
}