using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools;

internal class SpecializedToolsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithFactory<SpecializedToolViewModelV2>();
    }
}