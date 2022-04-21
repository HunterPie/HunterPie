using HunterPie.Core.Client.Configuration.Integrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Client.Configuration.Games
{
    public class MHRConfig
    {
        public DiscordRichPresence RichPresence { get; set; } = new();
        public OverlayConfig Overlay { get; set; } = new();
    }
}
