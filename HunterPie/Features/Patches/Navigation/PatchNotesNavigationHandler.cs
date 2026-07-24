using HunterPie.Core.Client.Localization;
using HunterPie.DI;
using HunterPie.Features.Patches.ViewModels;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Client.Sidebar.Entity;
using HunterPie.UI.Client.Sidebar.Handler;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Features.Patches.Navigation;

internal class PatchNotesNavigationHandler(
    IBodyNavigator bodyNavigator,
    ILocalizationRepository localizationRepository,
    IDependencyRegistry dependencies
) : NavigationHandler.View<PatchesViewModel>(
    label: localizationRepository.FindStringBy("//Strings/Client/Tabs/Tab[@Id='PATCH_NOTES_STRING']"),
    icon: Resources.Icon("ICON_DOCUMENTATION")
)
{
    public override Task InitializeAsync()
    {
        State = SideBarButtonState.Enabled;
        return Task.CompletedTask;
    }

    public override Task ExecuteAsync()
    {
        PatchesViewModel viewModel = dependencies.Get<PatchesViewModel>();

        bodyNavigator.Navigate(viewModel);

        return Task.CompletedTask;
    }
}