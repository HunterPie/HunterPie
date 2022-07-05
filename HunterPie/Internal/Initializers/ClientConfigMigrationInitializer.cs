using Config = HunterPie.Core.Client.Configuration.Config;
using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Versions;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Domain.Interfaces;
using HunterPie.Internal.Migrations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using HunterPie.Core.Json;

namespace HunterPie.Internal.Initializers
{
    internal class ClientConfigMigrationInitializer : IInitializer
    {
        private readonly static Dictionary<int, ISettingsMigrator> _migrators = new()
        {
            { 0, new V2SettingsMigrator() },
            { 1, new V3SettingsMigrator() },
        };

        public void Init()
        {
            CreateConfigIfNeeded();

            IVersionedConfig versionedConfig = ReadSettingsAs<VersionedConfig>();

            if (!_migrators.ContainsKey(versionedConfig.Version))
                return;

            ISettingsMigrator migrator = _migrators[versionedConfig.Version];
            versionedConfig = ReadSettingsAs<IVersionedConfig>(migrator.GetRequiredType());

            while (_migrators.ContainsKey(versionedConfig.Version))
            {
                if (!migrator.CanMigrate(versionedConfig))
                    return;

               versionedConfig = migrator.Migrate(versionedConfig);
            }

            RewriteSettings(versionedConfig);
        }

        private void CreateConfigIfNeeded()
        {
            string configPath = ClientInfo.GetPathFor(ClientConfig.CONFIG_NAME);

            if (!File.Exists(configPath))
                ConfigHelper.WriteObject(configPath, new Config());
        }

        private T ReadSettingsAs<T>()
        {
            return ReadSettingsAs<T>(typeof(T));
        }

        private T ReadSettingsAs<T>(Type type)
        {
            string rawSettings = File.ReadAllText(
                ClientInfo.GetPathFor(ClientInfo.ConfigName)
            );

            return (T)JsonConvert.DeserializeObject(rawSettings, type);
        }

        private void RewriteSettings(IVersionedConfig newSettings)
        {
            var serializerSettings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
            string serializedConfig = JsonProvider.Serialize(newSettings);
            
            File.WriteAllText(
                ClientInfo.GetPathFor(ClientInfo.ConfigName), 
                serializedConfig
            );
        }
    }
}
