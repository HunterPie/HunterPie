using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise
{
    internal class MeowcenariesContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly MHRPlayer _player;
        public readonly MeowcenariesViewModel ViewModel = new();

        public MeowcenariesContextHandler(MHRContext context)
        {
            _context = context;
            _player = context.Game.Player as MHRPlayer;
            UpdateData();
        }

        public void HookEvents()
        {
            _player.Meowmasters.OnDeployStateChange += OnDeployStateChange;
            _player.Meowmasters.OnStepChange += OnStepChange;
            _player.Meowmasters.OnBuddyCountChange += OnBuddyCountChange;
        }

        public void UnhookEvents()
        {
            _player.Meowmasters.OnDeployStateChange -= OnDeployStateChange;
            _player.Meowmasters.OnStepChange -= OnStepChange;
            _player.Meowmasters.OnBuddyCountChange -= OnBuddyCountChange;
        }
        
        private void UpdateData()
        {
            ViewModel.Step = _player.Meowmasters.Step;
            ViewModel.MaxSteps = _player.Meowmasters.MaxSteps;
            ViewModel.ExpectedOutcome = _player.Meowmasters.ExpectedOutcome;
            ViewModel.MaxOutcome = _player.Meowmasters.MaxOutcome;
            ViewModel.BuddyCount = _player.Meowmasters.BuddyCount;
            ViewModel.MaxBuddyCount = _player.Meowmasters.MaxBuddies;
            ViewModel.IsDeployed = _player.Meowmasters.IsDeployed;
        }
        
        private void OnBuddyCountChange(object sender, MHRMeowmasters e)
        {
            ViewModel.MaxBuddyCount = e.MaxBuddies;
            ViewModel.BuddyCount = e.BuddyCount;
        }

        private void OnStepChange(object sender, MHRMeowmasters e)
        {
            ViewModel.Step = e.Step;
            ViewModel.MaxSteps = e.MaxSteps;
        }


        private void OnDeployStateChange(object sender, MHRMeowmasters e)
        {
            ViewModel.IsDeployed = e.IsDeployed;
        }

    }
}
