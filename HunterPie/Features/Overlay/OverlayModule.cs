using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Overlay.Services;
using HunterPie.Features.Overlay.Widgets;
using HunterPie.Integrations.Datasources.Common.Monster;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using System.Numerics;

namespace HunterPie.Features.Overlay;

internal class OverlayModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<WidgetDataTemplateProvider>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        registry
            .WithSingle<OverlayManager>()
            .WithSingle<AbnormalitiesWidgetInitializer>()
            .WithSingle<ActivitiesWidgetInitializer>()
            .WithSingle<ChatWidgetInitializer>()
            .WithSingle<ClassWidgetInitializer>()
            .WithSingle<ClockWidgetInitializer>()
            .WithSingle<DamageWidgetInitializer>()
            .WithSingle<MonsterWidgetInitializer>()
            .WithSingle<PlayerHudWidgetInitializer>()
            .WithSingle<SpecializedToolWidgetInitializer>()
            .WithSingle<WirebugWidgetInitializer>();

        registry
            .WithSingle<DistanceFunc>(r => r.Get<IContext>() switch
            {
                MHWContext => static (Vector3 playerPosition, Vector3 monsterPosition) => Vector3.Distance(playerPosition, monsterPosition) / 100.0f,
                _ => Vector3.Distance
            })
            .WithSingle<WeightedTargetDetectionService>();
    }
}