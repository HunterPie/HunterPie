using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.Game.World;
using HunterPie.Core.Game.World.Entities;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.SpecializedTools;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Overlay
{
    internal class SpecializedToolWidgetInitializer : IWidgetInitializer
    {
        readonly List<IContextHandler> _handlers = new(2);

        public void Load(Context context)
        {
            if (context is MHWContext mhwContext)
            {
                InitializePrimaryTool(mhwContext);
                InitializeSecondaryTool(mhwContext);
            }
            
        }

        public void Unload()
        {
            foreach (IContextHandler handler in _handlers)
                handler?.UnhookEvents();

        }

        private void InitializePrimaryTool(MHWContext context)
        {
            SpecializedToolWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
                GameProcess.MonsterHunterWorld, 
                config =>
                {
                    return config.PrimarySpecializedToolWidget;
                }
            );

            if (!config.Initialize)
                return;

            MHWPlayer player = (MHWPlayer)context.Game.Player;

            _handlers.Add(new SpecializedToolWidgetContextHandler(
                context,
                player.Tools.ElementAtOrDefault(0),
                config
            ));
        }

        private void InitializeSecondaryTool(MHWContext context)
        {
            SpecializedToolWidgetConfig config = ClientConfigHelper.DeferOverlayConfig(
                GameProcess.MonsterHunterWorld,
                config =>
                {
                    return config.SecondarySpecializedToolWidget;
                }
            );

            if (!config.Initialize)
                return;

            MHWPlayer player = (MHWPlayer)context.Game.Player;

            _handlers.Add(new SpecializedToolWidgetContextHandler(
                context,
                player.Tools.ElementAtOrDefault(1),
                config
            ));
        }
    }
}
