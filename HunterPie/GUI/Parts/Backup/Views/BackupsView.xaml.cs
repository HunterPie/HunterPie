using HunterPie.GUI.Parts.Backup.ViewModels;
using HunterPie.UI.Architecture;

namespace HunterPie.GUI.Parts.Backup.Views;
/// <summary>
/// Interaction logic for BackupsView.xaml
/// </summary>
public partial class BackupsView : View<BackupsViewModel>
{
    public BackupsView()
    {
        InitializeComponent();
    }

    protected override void Initialize()
    {
        ViewModel.FetchBackups();
    }
}
