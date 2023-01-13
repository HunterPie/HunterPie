using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Integrations.Datasources.MonsterHunterWorld;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using HunterPie.UI.Overlay.Widgets.Activities.World;
using System;

namespace HunterPie.UI.Overlay.Widgets.Activities;
public class WorldActivitiesWidgetContextHandler : IContextHandler
{

    private readonly MHWPlayer _player;
    private readonly IContextHandler[] _handlers;
    private readonly ActivitiesViewModel _viewModel;
    private readonly ActivitiesView _view;

    private readonly HarvestBoxContextHandler _harvestBoxContextHandler;

    public WorldActivitiesWidgetContextHandler(MHWContext context)
    {
        _player = (MHWPlayer)context.Game.Player;

        _view = new ActivitiesView(ClientConfig.Config.World.Overlay.ActivitiesWidget);

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(_view);

        _viewModel = _view.ViewModel;

        _harvestBoxContextHandler = new(context);

        _handlers = new IContextHandler[] { _harvestBoxContextHandler };

        UpdateData();
    }

    public void HookEvents()
    {
        foreach (IContextHandler handler in _handlers)
            handler.HookEvents();

        _viewModel.Activities.Add(_harvestBoxContextHandler.ViewModel);

        _player.OnStageUpdate += OnStageUpdate;
    }

    private void UpdateData() =>
        _viewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;

    private void OnStageUpdate(object sender, EventArgs e) =>
        _viewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;

    public void UnhookEvents()
    {
        foreach (IContextHandler handler in _handlers)
            handler.UnhookEvents();

        _player.OnStageUpdate -= OnStageUpdate;

        WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(_view);
    }
}
