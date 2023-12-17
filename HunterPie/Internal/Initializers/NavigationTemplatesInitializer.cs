using HunterPie.Domain.Interfaces;
using HunterPie.GUI.Parts.Patches.ViewModels;
using HunterPie.GUI.Parts.Patches.Views;
using HunterPie.GUI.Parts.Settings.ViewModels;
using HunterPie.GUI.Parts.Settings.Views;
using HunterPie.UI.Navigation;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class NavigationTemplatesInitializer : IInitializer
{
    public Task Init()
    {
        NavigationProvider.Register<SettingsView, SettingsViewModel>();
        NavigationProvider.Register<PatchesView, PatchesViewModel>();

        return Task.CompletedTask;
    }
}