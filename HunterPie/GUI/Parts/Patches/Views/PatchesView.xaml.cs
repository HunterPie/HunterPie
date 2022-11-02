using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Patches.Views;
/// <summary>
/// Interaction logic for PatchesView.xaml
/// </summary>
public partial class PatchesView : View<PatchesViewModel>
{
    public PatchesView()
    {
        InitializeComponent();
    }

    protected override void Initialize() => ViewModel.FetchPatchesAsync();
}
