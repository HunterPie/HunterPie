using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Settings.Factory;

namespace HunterPie.Features.Settings;

internal class SettingsModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<SettingsFactory>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        IContext ctx = registry.Get<IContext>();
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ctx.Process.Type);

        registry
            .WithSingle(_ => config)
            .WithSingle(_ => config.ActivitiesWidget)
            .WithSingle(_ => config.InsectGlaiveWidget)
            .WithSingle(_ => config.SwitchAxeWidget)
            .WithSingle(_ => config.BossesWidget)
            .WithSingle(_ => config.ChargeBladeWidget)
            .WithSingle(_ => config.ChatWidget)
            .WithSingle(_ => config.ClockWidget)
            .WithSingle(_ => config.DamageMeterWidget)
            .WithSingle(_ => config.DebugWidget)
            .WithSingle(_ => config.DualBladesWidget)
            .WithSingle(_ => config.PrimarySpecializedToolWidget)
            .WithSingle(_ => config.SecondarySpecializedToolWidget)
            .WithSingle(_ => config.WirebugWidget);
    }
}