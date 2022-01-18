using HunterPie.Core.Settings;
using HunterPie.Core.Domain.Constants;
using HunterPie.Core.Architecture;

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
    }
}
