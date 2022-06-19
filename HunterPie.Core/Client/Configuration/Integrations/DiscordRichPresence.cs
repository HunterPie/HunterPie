using HunterPie.Core.Architecture;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Settings;

namespace HunterPie.Core.Client.Configuration.Integrations
{
    [SettingsGroup("DISCORD_RPC_STRING", "ICON_RPC", availableGames: GameProcess.MonsterHunterWorld | GameProcess.MonsterHunterRise)]
    public class DiscordRichPresence : ISettings
    {
        [SettingField("DRPC_ENABLE_RICH_PRESENCE")]
        public Observable<bool> EnableRichPresence { get; set; } = true;

        [SettingField("DRPC_ENABLE_SHOW_CHARACTER_INFO")]
        public Observable<bool> ShowCharacterInfo { get; set; } = true;

        [SettingField("DRPC_ENABLE_SHOW_MONSTER_HEALTH")]
        public Observable<bool> ShowMonsterHealth { get; set; } = true;
    }
}
