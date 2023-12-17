using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using HunterPie.UI.SideBar.ViewModels;
using System;

namespace HunterPie.Domain.Sidebar.Elements;

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
        Navigator.Navigate(new PatchesViewModel());
    }
}