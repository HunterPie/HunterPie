using HunterPie.DI;
using HunterPie.DI.Modules;
using HunterPie.UI.Header.ViewModels;

namespace HunterPie.UI.Header;

internal class HeaderModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<HeaderViewModel>();
    }
}