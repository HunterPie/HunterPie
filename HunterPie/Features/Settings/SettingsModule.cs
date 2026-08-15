using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Games;
using HunterPie.Core.Game;
using HunterPie.DI;
using HunterPie.DI.Module;
using HunterPie.Features.Settings.Factory;
using HunterPie.Features.Settings.Navigation;
using ConfigHolder = HunterPie.Core.Client.ClientConfig;

namespace HunterPie.Features.Settings;

internal class SettingsModule : IDependencyModule, IScopedModule
{
    void IDependencyModule.Register(IDependencyRegistry registry)
    {
        registry
            .WithSingle<SettingsFactory>()
            .WithSingle(static _ => ConfigHolder.Config.Client)
            .WithSingle(static _ => ConfigHolder.Config)
            .WithSingle(static _ => ConfigHolder.Config.Development)
            .WithSingle<SettingsNavigationHandler>();
    }

    void IScopedModule.Register(IScopedDependencyRegistry registry)
    {
        IContext ctx = registry.Get<IContext>();
        GameConfig config = ClientConfigHelper.GetGameConfigBy(ctx.Process.Type);

        registry
            .WithSingle(_ => config)
            .WithSingle(_ => config.Overlay)
            .WithSingle(_ => config.RichPresence);
    }
}