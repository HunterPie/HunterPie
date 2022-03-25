using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Client
{
    public interface IChat
    {
        public List<IChatMessage> Messages { get; }
        public bool IsChatOpen { get; }

        public event EventHandler<IChatMessage> OnNewChatMessage;
        public event EventHandler<IChat> OnChatOpen;
    }
}
