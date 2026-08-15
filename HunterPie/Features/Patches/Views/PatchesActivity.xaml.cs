using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Architecture.Views;
using System.Windows;

namespace HunterPie.Features.Patches.Views;
/// <summary>
/// Interaction logic for PatchesActivity.xaml
/// </summary>
[View<PatchesViewModel>]
internal partial class PatchesActivity : Activity
{
    public PatchesActivity()
    {
        InitializeComponent();
    }

    private async void OnLoaded(object _, RoutedEventArgs __)
    {
        await ViewModel.FetchPatchesAsync();
    }
}