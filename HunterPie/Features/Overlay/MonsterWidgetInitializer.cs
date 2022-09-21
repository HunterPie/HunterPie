﻿using HunterPie.Core.Client;
using HunterPie.Core.Game;
using HunterPie.Core.System;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster;

namespace HunterPie.Features.Overlay;

internal class MonsterWidgetInitializer : IWidgetInitializer
{
    private IContextHandler _handler;

    public void Load(Context context)
    {
        Core.Client.Configuration.OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(ProcessManager.Game);

        if (!config.BossesWidget.Initialize)
            return;

        _handler = new MonsterWidgetContextHandler(context);
    }

    public void Unload() => _handler?.UnhookEvents();
}
