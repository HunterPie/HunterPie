using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Abnormality;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Features.Overlay;

internal class AbnormalitiesWidgetInitializer : IWidgetInitializer
{
    private readonly List<IContextHandler> _handlers = new();

    public void Load(IContext context)
    {
        Core.Client.Configuration.OverlayConfig config = ProcessManager.Game switch
        {
            GameProcess.MonsterHunterRise => ClientConfig.Config.Rise.Overlay,
            GameProcess.MonsterHunterWorld => ClientConfig.Config.World.Overlay,
            _ => throw new System.NotImplementedException(),
        };

        Core.Client.Configuration.Overlay.AbnormalityWidgetConfig[] configs = config.AbnormalityTray.Trays.Trays.ToArray();
        for (int i = 0; i < config.AbnormalityTray.Trays.Trays.Count; i++)
        {
            ref Core.Client.Configuration.Overlay.AbnormalityWidgetConfig abnormConfig = ref configs[i];

            if (!abnormConfig.Initialize)
                continue;

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
