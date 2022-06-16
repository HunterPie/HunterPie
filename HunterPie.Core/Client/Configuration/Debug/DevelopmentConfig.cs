using HunterPie.Core.Settings;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Architecture;
using HunterPie.Core.Client.Configuration.Enums;
using System.Diagnostics;

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

        [SettingField("DEV_MOCK_WIREBUG_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockWirebugWidget { get; set; } = false;

        [SettingField("DEV_MOCK_ACTIVITIES_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockActivitiesWidget { get; set; } = false;

        [SettingField("DEV_MOCK_CHAT_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockChatWidget { get; set; } = false;

        [SettingField("DEV_MOCK_SPECIALIZED_TOOL_WIDGET_STRING", requiresRestart: true)]
        public Observable<bool> MockSpecializedToolWidget { get; set; } = false;

        [SettingField("DEV_POOGIE_API_HOST_STRING")]
        public Observable<string> PoogieApiHost { get; set; } = "";

        [SettingField("DEV_LOG_LEVEL")]
        public Observable<LogLevel> ClientLogLevel { get; set; } = LogLevel.Info;

        [SettingField("DEV_PRESENTATION_TRACER_LEVEL")]
        public Observable<SourceLevels> PresentationSourceLevel { get; set; } = SourceLevels.Off;
    }
}
