using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.UI.Overlay.Widgets.Activities.Rise;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Activities
{
    // TODO: Separate this into activity IContextHandlers
    public class RiseActivitiesWidgetContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly MHRPlayer _player;
        private readonly IContextHandler[] _handlers;
        private readonly ActivitiesViewModel ViewModel;
        private readonly ActivitiesView View;

        private readonly SubmarinesContextHandler _submarinesHandler;
        private readonly TrainingDojoContextHandler _trainingDojoContextHandler;
        private readonly MeowcenariesContextHandler _meowcenariesContextHandler;
        private readonly CohootContextHandler _cohootContextHandler;

        public RiseActivitiesWidgetContextHandler(MHRContext context)
        {
            _context = context;
            _player = (MHRPlayer)context.Game.Player;

            View = new ActivitiesView(ClientConfig.Config.Rise.Overlay.ActivitiesWidget);
            WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(View);

            ViewModel = View.ViewModel;
            
            _submarinesHandler = new(_context);
            _trainingDojoContextHandler = new(_context);
            _meowcenariesContextHandler = new(_context);
            _cohootContextHandler = new(_context);

            _handlers = new IContextHandler[] 
            { 
                _submarinesHandler, 
                _trainingDojoContextHandler, 
                _meowcenariesContextHandler,
                _cohootContextHandler,
            };
            UpdateData();
            HookEvents();
        }

        public void HookEvents()
        {
            foreach (IContextHandler handler in _handlers)
                handler.HookEvents();

            ViewModel.Activities.Add(_submarinesHandler.ViewModel);
            ViewModel.Activities.Add(_trainingDojoContextHandler.ViewModel);
            ViewModel.Activities.Add(_meowcenariesContextHandler.ViewModel);
            ViewModel.Activities.Add(_cohootContextHandler.ViewModel);

            _player.OnStageUpdate += OnStageChange;
        }

        private void UpdateData()
        {
            ViewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;
        }

        private void OnStageChange(object sender, EventArgs e)
        {
            ViewModel.InVisibleStage = !_player.InHuntingZone && _player.StageId != -1;
        }

        public void UnhookEvents()
        {
            foreach (IContextHandler handler in _handlers)
                handler.UnhookEvents();

            _player.OnStageUpdate -= OnStageChange;
            WidgetManager.Unregister<ActivitiesView, ActivitiesWidgetConfig>(View);
        }
                
    }
}
