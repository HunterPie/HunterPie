using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Debug.Mocks;

namespace HunterPie.Features.Debug;

internal class DebugModule : IDependencyModule
{
    public void Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<AbnormalityWidgetMocker>()
            .WithSingle<ActivitiesWidgetMocker>()
            .WithSingle<ChargeBladeWidgetMocker>()
            .WithSingle<ChatWidgetMocker>()
            .WithSingle<ClockWidgetMocker>()
            .WithSingle<DamageWidgetMocker>()
            .WithSingle<DualBladesWidgetMocker>()
            .WithSingle<InsectGlaiveWidgetMocker>()
            .WithSingle<LongSwordWidgetMocker>()
            .WithSingle<MonsterWidgetMocker>()
            .WithSingle<PlayerHudWidgetMocker>()
            .WithSingle<SpecializedToolWidgetMocker>()
            .WithSingle<SwitchAxeWidgetMocker>()
            .WithSingle<WirebugWidgetMocker>()
            .WithSingle<WidgetMocksProvider>();
    }
}