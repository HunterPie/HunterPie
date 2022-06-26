
using DiscordRPC;
using HunterPie.Core.Game.Rise;
using System;
using System.Linq;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client;
using HunterPie.Core.Game.Demos.Sunbreak;
using HunterPie.Core.Logger;

namespace HunterPie.Integrations.Discord
{
    internal sealed class RiseSunbreakDemoRichPresence : RichPresence
    {
        private const string RISE_APP_ID = "932399108017242182";

        private readonly MHRSunbreakDemoGame game;
        protected override DiscordRichPresence Settings => ClientConfig.Config.Rise.RichPresence;

        public RiseSunbreakDemoRichPresence(MHRSunbreakDemoContext context) : base(RISE_APP_ID, context.Game)
        {
            game = (MHRSunbreakDemoGame)context.Game;
        }

        protected override void HandlePresence()
        {
            string description = null;

            description = game.Player.StageId switch
            {
                -1 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_MAIN_MENU']"),
                >= 0 and <= 4 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_IDLE']"),
                5 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_PRACTICE']"),
                207 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_RAMPAGE']"),
                199 => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_STATE_CHAR_SELECTION']"),
                _ => Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_EXPLORING']")
            };

            IMonster targetMonster = game.Monsters.FirstOrDefault(monster => monster.Target == Target.Self);
            if (targetMonster is not null)
            {
                string descriptionString = Settings.ShowMonsterHealth
                    ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_HUNTING']")
                    : Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_STATE_HUNTING_NO_HEALTH']");

                description = descriptionString
                    .Replace("{Monster}", targetMonster.Name)
                    .Replace("{Percentage}", $"{targetMonster.Health / targetMonster.MaxHealth * 100:0}");
            }

            Presence.WithDetails(description)
                .WithAssets(new Assets()
                {
                    LargeImageText = MHRSunbreakDemoContext.Strings.GetStageNameById(game.Player.StageId),
                    LargeImageKey = game.Player.StageId == -1
                                    ? "unknown"
                                    : $"rise-stage-{game.Player.StageId}",
                    SmallImageText = Settings.ShowCharacterInfo
                        ? Localization.QueryString("//Strings/Client/Integrations/Discord[@Id='DRPC_RISE_CHARACTER_STRING_FORMAT']")
                            .Replace("{Character}", game.Player.Name)
                            .Replace("{HighRank}", game.Player.HighRank.ToString())
                        : null,
                    SmallImageKey = "unknown"
                });

        }
    }
}
