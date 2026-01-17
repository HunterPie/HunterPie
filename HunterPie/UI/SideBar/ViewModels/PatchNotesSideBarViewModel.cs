using HunterPie.DI;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Architecture;
using HunterPie.UI.Navigation;
using System;
using System.Threading.Tasks;

namespace HunterPie.UI.SideBar.ViewModels;

internal class PatchNotesSideBarViewModel(IBodyNavigator bodyNavigator) : ViewModel, ISideBarViewModel
{
    private readonly IBodyNavigator _bodyNavigator = bodyNavigator;

    public Type Type => typeof(PatchesViewModel);

    public string Label => "//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']";

    public string Icon => "ICON_DOCUMENTATION";

    public bool IsAvailable => true;

    public bool IsSelected { get; set => SetValue(ref field, value); }

    public Task ExecuteAsync()
    {
        PatchesViewModel viewModel = DependencyContainer.Get<PatchesViewModel>();

        _bodyNavigator.Navigate(viewModel);

        return Task.CompletedTask;
    }
}