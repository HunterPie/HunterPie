using HunterPie.DI;
using HunterPie.DI.Module;

namespace HunterPie.Features.Input;

internal class InputModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<HotkeyService>();
    }
}