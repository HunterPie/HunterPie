using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Abnormality;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Overlay
{
    internal class AbnormalitiesWidgetInitializer : IWidgetInitializer
    {
        readonly List<IContextHandler> _handlers = new();

        public void Load(Context context)
        {
            var configs = ClientConfig.Config.Overlay.AbnormalityTray.Trays.Trays.ToArray();
            for (int i = 0; i < ClientConfig.Config.Overlay.AbnormalityTray.Trays.Trays.Count; i++)
            {
                ref var abnormConfig = ref configs[i];
                _handlers.Add(new AbnormalityWidgetContextHandler(context, ref abnormConfig));
            }
        }

        public void Unload()
        {
            foreach (IContextHandler handler in _handlers)
                handler.UnhookEvents();

            _handlers.Clear();
        }
        
    }
}
