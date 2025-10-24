using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;
using System;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;

internal class MHWildsActivitiesController : IContextHandler
{
    private readonly MHWildsContext _context;
    private readonly ActivitiesViewModel _viewModel;
    private readonly IContextHandler[] _contextHandlers;

    public MHWildsActivitiesController(
        MHWildsContext context,
        ActivitiesViewModel viewModel,
        MHWildsActivitiesViewModel activities,
        Dispatcher dispatcher)
    {
        _context = context;
        _viewModel = viewModel;
        _viewModel.Activities = activities;
        _contextHandlers = new IContextHandler[]
        {
            new MaterialRetrievalController(
                context: context,
                viewModel: activities.MaterialRetrieval,
                dispatcher: dispatcher
            ),
            new SupportShipController(
                context: context,
                viewModel: activities.SupportShip
            ),
            new IngredientsCenterController(
                context: context,
                viewModel: activities.IngredientsCenter
            )
        };
    }

    public void HookEvents()
    {
        _context.Game.Player.OnStageUpdate += OnStageUpdate;
        _context.Game.Player.OnVillageEnter += OnStageUpdate;
        _context.Game.Player.OnVillageLeave += OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.HookEvents();

        UpdateData();
    }


    public void UnhookEvents()
    {
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.UnhookEvents();
    }

    private void OnStageUpdate(object sender, EventArgs e) =>
        UpdateData();

    private void UpdateData()
    {
        _viewModel.InVisibleStage = !_context.Game.Player.InHuntingZone
                                    && _context.Game.Player.StageId != -1;
    }
}