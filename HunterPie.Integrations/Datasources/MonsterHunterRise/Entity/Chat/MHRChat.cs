using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Entity.Game.Chat;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Chat;

public class MHRChat : IChat, IEventDispatcher
{
    private bool _isChatOpen;
    private readonly Dictionary<long, MHRChatMessage> _messages = new();
    public List<IChatMessage> Messages => _messages.Values.ToList<IChatMessage>();

    public bool IsChatOpen
    {
        get => _isChatOpen;
        internal set
        {
            if (value != _isChatOpen)
            {
                _isChatOpen = value;
                this.Dispatch(OnChatOpen, this);
            }
        }
    }

    public event EventHandler<IChatMessage>? OnNewChatMessage;
    public event EventHandler<IChat>? OnChatOpen;

    internal bool ContainsMessage(long messageAddress) => _messages.ContainsKey(messageAddress);

    internal void AddMessage(long messageAddress, MHRChatMessage message)
    {
        if (_messages.ContainsKey(messageAddress))
            return;

        _messages.Add(messageAddress, message);

        this.Dispatch(OnNewChatMessage, message);
    }
}
