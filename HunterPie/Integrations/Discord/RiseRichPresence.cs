
using DiscordRPC;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Rise;
using System;
using System.Linq;
using DiscordRPC.Message;
using HunterPie.Core.Logger;
using System.Timers;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client;
using System.ComponentModel;
using HunterPie.Core.Client.Localization;

namespace HunterPie.Integrations.Discord
{
    internal class RiseRichPresence : IDisposable
    {
        const string AppId = "932399108017242182";
        private MHRGame game;
        private Timestamps locationTime = Timestamps.Now;
        private readonly RichPresence presence = new();
        private readonly DiscordRpcClient client = new(AppId, autoEvents: true);
        private DiscordRichPresence Settings => ClientConfig.Config.RichPresence;

        private readonly Timer timer = new(10000)
        {
            AutoReset = true
        };

        public RiseRichPresence(Context context)
        {
            game = (MHRGame)context.Game;

            HookEvents();
            timer.Start();

            if (Settings.EnableRichPresence)
                client.Initialize();
        }

        private void HookEvents()
        {
            client.OnReady += OnReady;
            Settings.EnableRichPresence.PropertyChanged += OnEnableRichPresenceChanged;
            game.Player.OnStageUpdate += OnStageUpdate;
            timer.Elapsed += OnTick;
        }

        private void UnhookEvents()
        {
            client.OnReady -= OnReady;
            game.Player.OnStageUpdate -= OnStageUpdate;
            Settings.EnableRichPresence.PropertyChanged -= OnEnableRichPresenceChanged;
            timer.Elapsed -= OnTick;
        }

        private void OnEnableRichPresenceChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Settings.EnableRichPresence)
                UpdatePresence();
            else
                client.ClearPresence();
        }

        private void OnReady(object sender, ReadyMessage args)
        {
            Log.Info($"Connected to Discord: {args.User}");
            UpdatePresence();
        }

        private void OnTick(object sender, ElapsedEventArgs e) => UpdatePresence();
        private void OnStageUpdate(object sender, EventArgs e)
        {
            locationTime = Timestamps.Now;
            UpdatePresence();
        }

        private void UpdatePresence()
        {
            if (!Settings.EnableRichPresence || client is null || !client.IsInitialized)
            {
                client.ClearPresence();
                return;
            }

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
            
            presence.WithDetails(description)
                .WithState(null)
                .WithParty(null)
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
                .WithTimestamps(locationTime);

            client.SetPresence(presence);
        }

        public void Dispose()
        {
            UnhookEvents();
            client.ClearPresence();
            client.Dispose();
            timer.Stop();
            timer.Dispose();
        }
    }
}
