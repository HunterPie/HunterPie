using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Integrations.Datasources.MonsterHunterWilds;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModels;
using HunterPie.UI.Overlay.Widgets.Activities.Views;
using HunterPie.UI.Overlay.Widgets.Activities.Wilds.ViewModels;
using System;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities.Wilds.Controllers;

internal class MHWildsActivitiesController : IContextHandler
{
    private readonly MHWildsContext _context;
    private readonly ActivitiesView _view;
    private readonly ActivitiesViewModel _viewModel;
    private readonly IContextHandler[] _contextHandlers;

    public MHWildsActivitiesController(
        MHWildsContext context,
        ActivitiesView view,
        MHWildsActivitiesViewModel activities,
        Dispatcher dispatcher)
    {
        _context = context;
        _view = view;
        _viewModel = view.ViewModel;
        _viewModel.Activities = activities;
        _contextHandlers = new IContextHandler[]
        {
            new MaterialRetrievalController(
                context: context,
                viewModel: activities.MaterialRetrieval,
                dispatcher: dispatcher
            ),
        };
    }

    public void HookEvents()
    {
        _context.Game.Player.OnStageUpdate += OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.HookEvents();

        UpdateData();

        WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    public void UnhookEvents()
    {
        _context.Game.Player.OnStageUpdate -= OnStageUpdate;

        foreach (IContextHandler handler in _contextHandlers)
            handler.UnhookEvents();

        WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(
            widget: _view
        );
    }

    private void OnStageUpdate(object sender, EventArgs e) =>
        UpdateData();

    private void UpdateData()
    {
        _viewModel.InVisibleStage = !_context.Game.Player.InHuntingZone
                                    && _context.Game.Player.StageId != -1;
    }
}