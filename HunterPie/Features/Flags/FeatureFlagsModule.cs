using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Flags.Repository;

namespace HunterPie.Features.Flags;

internal class FeatureFlagsModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<DefaultFeatureFlags>()
            .WithSingle(static (r) => new LocalFeatureFlagRepository(
                    flags: r.Get<DefaultFeatureFlags>().ReadOnlyFlags
                )
            );
    }
}