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

namespace HunterPie.Integrations.Discord
{
    internal class RiseRichPresence : IDisposable
    {
        const string AppId = "932399108017242182";
        private MHRGame game;
        private Timestamps locationTime = Timestamps.Now;
        private readonly RichPresence presence = new();
        private readonly DiscordRpcClient client = new(AppId, autoEvents: true);
        private readonly Timer timer = new(10000)
        {
            AutoReset = true
        };

        public RiseRichPresence(Context context)
        {
            game = (MHRGame)context.Game;

            HookEvents();
            timer.Start();
            client.Initialize();
        }

        private void HookEvents()
        {
            client.OnReady += OnReady;
            game.Player.OnStageUpdate += OnStageUpdate;
            timer.Elapsed += OnTick;
        }

        private void UnhookEvents()
        {
            client.OnReady -= OnReady;
            game.Player.OnStageUpdate -= OnStageUpdate;
            timer.Elapsed -= OnTick;
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
            string description = null;

            description = game.Player.StageId switch
            {
                -1 => "In Main Menu",
                >= 1 and <= 4 => "Chilling",
                5 => "Practicing",
                207 => "In Rampage",
                _ => "Exploring"
            };

            IMonster targetMonster = game.Monsters.FirstOrDefault(monster => monster.Target == Target.Self);
            if (targetMonster is not null)
                description = $"Hunting {targetMonster.Name} ({targetMonster.Health / targetMonster.MaxHealth * 100:0}%)";
            
            presence.WithDetails(description)
                .WithState(null)
                .WithParty(null)
                .WithAssets(new Assets()
                {
                    LargeImageText = MHRContext.Strings.GetStageNameById(game.Player.StageId),
                    LargeImageKey = game.Player.StageId == -1 
                                    ? "unknown" 
                                    : $"rise-stage-{game.Player.StageId}",
                    SmallImageText = game.Player.Name,
                    SmallImageKey = game.Player.WeaponId switch
                    {
                        Weapon.None => "unknown",   
                        _ => Enum.GetName(typeof(Weapon), game.Player.WeaponId)?.ToLower() ?? "unknown",
                    }
                })
                .WithTimestamps(locationTime);

            client.SetPresence(presence);
        }

        public void Dispose()
        {
            UnhookEvents();
            client.Dispose();
            timer.Stop();
            timer.Dispose();
        }
    }
}
