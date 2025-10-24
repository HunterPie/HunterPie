using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Interfaces;
using System;

namespace HunterPie.Internal.Migrations;

#pragma warning disable CS0612
internal class V5SettingsMigrator : ISettingsMigrator
{
    public bool CanMigrate(IVersionedConfig oldSettings) => oldSettings.GetType() == typeof(V4Config);

    public Type GetRequiredType() => typeof(V4Config);

    public IVersionedConfig Migrate(IVersionedConfig oldSettings)
    {
        if (oldSettings is not V4Config config)
            throw new InvalidCastException($"old config must be of type {typeof(V4Config)}, but was {oldSettings.GetType()}");

        var newConfig = new V5Config
        {
            Client = config.Client,
            Rise = config.Rise,
            World = config.World,
            Wilds = config.Wilds,
            Overlay = config.Overlay,
            Development = config.Development
        };


        if (newConfig.Rise.Overlay.DamageMeterWidget.IsPlotSlidingWindowEnabled.Value)
            newConfig.Rise.Overlay.DamageMeterWidget.DamagePlotStrategy.Value =
                DamagePlotStrategy.MovingAverageDamagePerSecond;

        if (newConfig.World.Overlay.DamageMeterWidget.IsPlotSlidingWindowEnabled.Value)
            newConfig.World.Overlay.DamageMeterWidget.DamagePlotStrategy.Value =
                DamagePlotStrategy.MovingAverageDamagePerSecond;

        if (newConfig.Wilds.Overlay.DamageMeterWidget.IsPlotSlidingWindowEnabled.Value)
            newConfig.Wilds.Overlay.DamageMeterWidget.DamagePlotStrategy.Value =
                DamagePlotStrategy.MovingAverageDamagePerSecond;

        return newConfig;
    }
}
#pragma warning restore CS0612