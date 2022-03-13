using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.Core.Game.Rise.Entities.Entity;
using HunterPie.UI.Overlay.Widgets.Activities.Rise;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities
{
    // TODO: Separate this into activity IContextHandlers
    public class RiseActivitiesWidgetContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly MHRPlayer _player;
        private readonly IContextHandler[] _handlers;
        private readonly ActivitiesViewModel ViewModel;

        private readonly SubmarinesContextHandler _submarinesHandler;
        private readonly TrainingDojoContextHandler _trainingDojoContextHandler;

        public RiseActivitiesWidgetContextHandler(MHRContext context)
        {
            _context = context;
            _player = (MHRPlayer)context.Game.Player;

            var widget = new ActivitiesView();
            WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(widget);

            ViewModel = widget.ViewModel;
            
            _submarinesHandler = new(_context);
            _trainingDojoContextHandler = new(_context);

            _handlers = new IContextHandler[] { _submarinesHandler, _trainingDojoContextHandler };
            UpdateData();
            HookEvents();
        }

        public void HookEvents()
        {
            foreach (IContextHandler handler in _handlers)
                handler.HookEvents();

            ViewModel.Activities.Add(_submarinesHandler.ViewModel);
            ViewModel.Activities.Add(_trainingDojoContextHandler.ViewModel);

            _player.OnStageUpdate += OnStageChange;
        }

        private void UpdateData()
        {
            ViewModel.InVisibleStage = MHRGame.VillageStages.Contains(_player.StageId);
        }

        private void OnStageChange(object sender, EventArgs e)
        {
            ViewModel.InVisibleStage = MHRGame.VillageStages.Contains(_player.StageId);
        }

        public void UnhookEvents()
        {
            foreach (IContextHandler handler in _handlers)
                handler.UnhookEvents();

            _player.OnStageUpdate -= OnStageChange;
        }
                
    }
}
