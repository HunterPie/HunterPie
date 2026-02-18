using DiscordRPC;
using HunterPie.Core.Client.Configuration.Integrations;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using System;
using System.Linq;

namespace HunterPie.Integrations.Discord.Strategies;

public class MHWildsDiscordPresenceStrategy(
    DiscordRichPresence configuration,
    ILocalizationRepository localizationRepository,
    IContext context) : IDiscordRichPresenceStrategy
{
    public string AppId => "1346929958949224518";

    private readonly DiscordRichPresence _configuration = configuration;
    private readonly ILocalizationRepository _localizationRepository = localizationRepository;
    private readonly IScopedLocalizationRepository _discordLocalizationRepository = localizationRepository.WithScope("//Strings/Client/Integrations/Discord");
    private readonly IContext _context = context;

    public void Update(RichPresence presence)
    {
        string description = BuildDescription();
        string state = BuildState();
        string stageName = _localizationRepository.FindStringBy(
            path: $"//Strings/Stages/Wilds/Stage[@Id='{_context.Game.Player.StageId}']"
        );
        bool isUnmappedStage = stageName.StartsWith("//Strings");

        presence
            .WithDetails(description)
            .WithState(state)
            .WithAssets(new Assets
            {
                LargeImageKey = isUnmappedStage switch
                {
                    true => "unknown",
                    _ => $"wilds-stage-{_context.Game.Player.StageId}"
                },
                LargeImageText = stageName,
                SmallImageKey = _context.Game.Player.Weapon.Id switch
                {
                    Weapon.None => null,
                    var id => Enum.GetName(typeof(Weapon), id)?.ToLowerInvariant()
                },
                SmallImageText = _configuration.ShowCharacterInfo.Value switch
                {
                    true => _discordLocalizationRepository.FindStringBy("DRPC_RISE_CHARACTER_STRING_FORMAT")
                        .Replace("{Character}", _context.Game.Player.Name)
                        .Replace("{HighRank}", _context.Game.Player.HighRank.ToString())
                        .Replace("{MasterRank}", "-"),
                    _ => null
                }
            })
            .WithParty(new Party
            {
                ID = _context.Game.Player.Name,
                Max = Math.Max(_context.Game.Player.Party.MaxSize, _context.Game.Player.Party.Size),
                Size = _context.Game.Player.Party.Size,
                Privacy = Party.PrivacySetting.Public
            });
    }

    private string BuildDescription()
    {
        string localizationStateId = (
                _context.Game.Player.StageId,
                _context.Game.Player.InHuntingZone) switch
        {
            (-1, _) => "DRPC_STATE_MAIN_MENU",
            (15, _) => "DRPC_RISE_STATE_PRACTICE",
            (_, false) => "DRPC_STATE_IDLE",
            _ => "DRPC_STATE_EXPLORING"
        };

        IMonster? targetMonster = _context.Game.Monsters.FirstOrDefault(it => it.Target == Target.Self);

        if (targetMonster is null)
            return _discordLocalizationRepository.FindStringBy(localizationStateId);

        string descriptionString = _configuration.ShowMonsterHealth
            ? "DRPC_STATE_HUNTING"
            : "DRPC_STATE_HUNTING_NO_HEALTH";

        return _discordLocalizationRepository.FindStringBy(descriptionString)
            .Replace("{Monster}", targetMonster.Name)
            .Replace("{Percentage}", $"{targetMonster.Health / targetMonster.MaxHealth * 100:0}");
    }

    private string BuildState()
    {
        string localizationId = _context.Game.Player.Party.Size <= 1
            ? "DRPC_PARTY_STATE_SOLO_STRING"
            : "DRPC_PARTY_STATE_GROUP_STRING";

        return _discordLocalizationRepository.FindStringBy(localizationId);
    }
}