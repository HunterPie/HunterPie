using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Native.Service;

namespace HunterPie.Features.Native;

internal class NativeModule : IScopedModule
{
    public void Register(IScopedDependencyRegistry registry)
    {
        registry.WithSingle<NativeInterfaceService>();
    }
}
