using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Rise.Entities.Chat
{
    public class MHRChat : IChat, IEventDispatcher
    {
        private readonly Dictionary<long, MHRChatMessage> _messages = new();
        public List<IChatMessage> Messages => _messages.Values.ToList<IChatMessage>();

        public event EventHandler<IChatMessage> OnNewChatMessage;

        internal void AddMessage(long messageAddress, MHRChatMessage message)
        {
            if (_messages.ContainsKey(messageAddress))
                return;

            _messages.Add(messageAddress, message);

            this.Dispatch(OnNewChatMessage, message);
        }
    }
}
