using HunterPie.Core.Settings;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;

namespace HunterPie.Core.Client.Configuration.Debug
{
    [SettingsGroup("DEV_TOOLS_STRING", "ICON_BUG", FeatureFlags.FEATURE_ADVANCED_DEV)]
    public class DevelopmentConfig : ISettings
    {
        [SettingField("DEV_MOCK_MONSTER_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockBossesWidget { get; set; } = false;

        [SettingField("DEV_MOCK_DAMAGE_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockDamageWidget { get; set; } = false;

        [SettingField("DEV_MOCK_ABNORMALITY_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockAbnormalityWidget { get; set; } = false;

        [SettingField("DEV_POOGIE_API_HOST_STRING")]
        public Observable<string> PoogieApiHost { get; set; } = "";

        [SettingField("DEV_ENABLE_DEBUG_MESSAGES")]
        public Observable<LogLevel> ClientLogLevel { get; set; } = LogLevel.Info;
    }
}
