using DiscordRPC;

namespace HunterPie.Integrations.Discord.Strategies;

internal class DisabledDiscordPresenceStrategy : IDiscordRichPresenceStrategy
{
    public string AppId => "";

    public void Update(RichPresence presence) { }
}