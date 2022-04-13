
using DiscordRPC;
using HunterPie.Core.Game.Rise;
using System;
using System.Linq;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Client.Localization;

namespace HunterPie.Integrations.Discord
{
    internal class RiseRichPresence : RichPresence
    {
        private const string RISE_APP_ID = "932399108017242182";

        private readonly MHRGame game;

        public RiseRichPresence(MHRContext context) : base(RISE_APP_ID, context.Game)
        {
            game = (MHRGame)context.Game;
        }

        protected override void HandlePresence()
        {
            string description = null;

            description = game.Player.StageId switch
            {
                -1 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_MAIN_MENU']"),
                >= 0 and <= 4 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_IDLE']"),
                5 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_PRACTICE']"),
                207 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_RAMPAGE']"),
                199 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_CHAR_SELECTION']"),
                _ => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_EXPLORING']")
            };

            IMonster targetMonster = game.Monsters.FirstOrDefault(monster => monster.Target == Target.Self);
            if (targetMonster is not null)
            {
                string descriptionString = Settings.ShowMonsterHealth
                    ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_HUNTING']")
                    : Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_HUNTING_NO_HEALTH']");

                description = descriptionString
                    .Replace("{Monster}", targetMonster.Name)
                    .Replace("{Percentage}", $"{targetMonster.Health / targetMonster.MaxHealth * 100:0}");
            }

            string state = game.Player.Party.Size <= 1
                ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_PARTY_STATE_SOLO_STRING']")
                : Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_PARTY_STATE_GROUP_STRING']");

            Presence.WithDetails(description)
                .WithAssets(new Assets()
                {
                    LargeImageText = MHRContext.Strings.GetStageNameById(game.Player.StageId),
                    LargeImageKey = game.Player.StageId == -1
                                    ? "unknown"
                                    : $"rise-stage-{game.Player.StageId}",
                    SmallImageText = Settings.ShowCharacterInfo
                        ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_CHARACTER_STRING_FORMAT']")
                            .Replace("{Character}", game.Player.Name)
                            .Replace("{HighRank}", game.Player.HighRank.ToString())
                        : null,
                    SmallImageKey = game.Player.WeaponId switch
                    {
                        Weapon.None => null,
                        _ => Enum.GetName(typeof(Weapon), game.Player.WeaponId)?.ToLower() ?? "unknown",
                    }
                })
                .WithParty(new Party()
                {
                    // TODO: Make shared ID for everyone in the party based on the session id
                    ID = game.Player.Name ?? "",
                    Max = game.Player.Party.MaxSize,
                    Size = game.Player.Party.Size,
                    Privacy = Party.PrivacySetting.Public
                })
                .WithState(state);

        }
    }
}
