﻿using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using HunterPie.UI.Architecture.Overlay;
using HunterPie.UI.Overlay.Widgets.Player;
using System.Threading.Tasks;

namespace HunterPie.Features.Overlay;

internal class PlayerHudWidgetInitializer : IWidgetInitializer
{
    private PlayerHudWidgetContextHandler? _handler;

    public GameProcessType SupportedGames => GameProcessType.MonsterHunterRise | GameProcessType.MonsterHunterWorld;

    public Task LoadAsync(IContext context)
    {
        OverlayConfig config = ClientConfigHelper.GetOverlayConfigFrom(context.Process.Type);

        if (!config.PlayerHudWidget.Initialize)
            return Task.CompletedTask;

        _handler = new PlayerHudWidgetContextHandler(context);

        return Task.CompletedTask;
    }

    public void Unload()
    {
        _handler?.UnhookEvents();
        _handler = null;
    }
}