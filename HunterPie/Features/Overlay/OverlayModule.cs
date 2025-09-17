using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Overlay.Services;
using HunterPie.Features.Overlay.Widgets;

namespace HunterPie.Features.Overlay;

internal class OverlayModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
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
            .WithSingle<WirebugWidgetInitializer>()
            .WithSingle<WidgetInitializers>()
            .WithSingle<WidgetDataTemplateProvider>();
    }
}