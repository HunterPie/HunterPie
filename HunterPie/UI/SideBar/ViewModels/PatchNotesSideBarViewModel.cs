using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;

namespace HunterPie.UI.SideBar.ViewModels;

internal class PatchNotesSideBarViewModel : ViewModel, ISideBarViewModel
{
    public Type Type => typeof(PatchesViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']";

    public string Icon => "ICON_DOCUMENTATION";

    public bool IsAvailable => true;

    private bool _isSelected;
    public bool IsSelected { get => _isSelected; set => SetValue(ref _isSelected, value); }

    public void Execute()
    {
        Navigator.Body.Navigate(new PatchesViewModel());
    }
}