using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Core.Game.Rise.Entities.Chat
{
    public class MHRChatMessage : IChatMessage
    {
        public string Message { get; init; }
        public string Author { get; init; }
        public AuthorType Type { get; init; }

        public int PlayerSlot { get; init; }
    }
}
