using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Interfaces;
using System;

namespace HunterPie.Internal.Migrations;

internal class V2SettingsMigrator : ISettingsMigrator
{
    public bool CanMigrate(IVersionedConfig oldSettings) => oldSettings.GetType() == typeof(Config);

    public IVersionedConfig Migrate(IVersionedConfig oldSettings)
    {
        if (oldSettings is Config config)
        {
#pragma warning disable CS0612 // Ignore obsolete since this is a setting migration code 
            OverlayClientConfig overlayClientConfig = new()
            {
                ToggleDesignMode = config.Overlay.ToggleDesignMode,
                HideWhenUnfocus = config.Overlay.HideWhenUnfocus,
            };
#pragma warning restore CS0612

            config.Overlay.DamageMeterWidget = new();

            var v2Config = new V2Config()
            {
                Client = config.Client,
                Rise = new()
                {
                    RichPresence = config.RichPresence,
                    Overlay = config.Overlay,
                },
                Overlay = overlayClientConfig,
                Development = config.Debug
            };

            return v2Config;
        }

        throw new InvalidCastException($"old config must be of type {typeof(Config)}, but was {oldSettings.GetType()}");
    }

    public Type GetRequiredType() => typeof(Config);
}