using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Observability.Tracing;

namespace HunterPie.Features.Observability;

internal class ObservabilityModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry.WithSingle<UITracer>();
    }
}
