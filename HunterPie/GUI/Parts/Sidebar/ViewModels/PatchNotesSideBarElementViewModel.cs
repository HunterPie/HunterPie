using HunterPie.Core.Architecture;
using HunterPie.GUI.Parts.Patches.Views;
using HunterPie.UI.Architecture.Navigator;
using HunterPie.UI.Assets.Application;
using System.Windows.Media;
using Localization = HunterPie.Core.Client.Localization.Localization;

namespace HunterPie.GUI.Parts.Sidebar.ViewModels;

internal class PatchNotesSideBarElementViewModel : Bindable, ISideBarElement
{
    private const string LAST_PATCH_NOTE_READ_KEY = "LastPatchNote";
    private bool _shouldNotify;

    public ImageSource Icon => Resources.Icon("ICON_DOCUMENTATION");
    public string Text => Localization.Query("//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']").Attributes["String"].Value;
    public bool IsActivable => true;
    public bool IsEnabled => true;
    public bool ShouldNotify { get => _shouldNotify; private set => SetValue(ref _shouldNotify, value); }

    public void ExecuteOnClick()
    {
        var view = new PatchesView();

        Navigator.Navigate(view);
    }
}
