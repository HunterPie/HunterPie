using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.Core.Game.Rise.Entities.Entity;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise
{
    internal class TrainingDojoContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly Dictionary<MHRBuddy, BuddyViewModel> _buddyViewModels;
        private MHRPlayer _player => (MHRPlayer)_context.Game.Player;

        public readonly TrainingDojoViewModel ViewModel = new();

        public TrainingDojoContextHandler(MHRContext context)
        {
            _context = context;
            _buddyViewModels = new(_player.TrainingDojo.Buddies.Length);

            UpdateData();
        }

        private void UpdateData()
        {
            ViewModel.Boosts = _player.TrainingDojo.Boosts;
            ViewModel.MaxBoosts = _player.TrainingDojo.MaxBoosts;
            ViewModel.Rounds = _player.TrainingDojo.Rounds;
            ViewModel.MaxRounds = _player.TrainingDojo.MaxRounds;
        }

        public void HookEvents()
        {
            _player.TrainingDojo.OnBoostsLeftChange += OnBoostsChange;
            _player.TrainingDojo.OnRoundsLeftChange += OnRoundsChange;

            foreach (MHRBuddy buddy in _player.TrainingDojo.Buddies)
            {
                if (_buddyViewModels.ContainsKey(buddy))
                    continue;

                _buddyViewModels[buddy] = new()
                {
                    Name = buddy.Name,
                    Level = buddy.Level,
                    IsEmpty = string.IsNullOrEmpty(buddy.Name)
                };

                buddy.OnNameChange += OnBuddyNameChange;
                buddy.OnLevelChange += OnBuddyLevelChange;
            }

            foreach (BuddyViewModel vm in _buddyViewModels.Values)
                ViewModel.Buddies.Add(vm);
        }

        public void UnhookEvents()
        {
            _player.TrainingDojo.OnBoostsLeftChange -= OnBoostsChange;
            _player.TrainingDojo.OnRoundsLeftChange -= OnRoundsChange;

            foreach (MHRBuddy buddy in _buddyViewModels.Keys)
            {
                buddy.OnNameChange -= OnBuddyNameChange;
                buddy.OnLevelChange -= OnBuddyLevelChange;
            }

            _buddyViewModels.Clear();
            ViewModel.Buddies.Clear();
        }

        private void OnRoundsChange(object sender, MHRTrainingDojo e)
        {
            ViewModel.Rounds = e.Rounds;
            ViewModel.MaxRounds = e.MaxRounds;
        }

        private void OnBoostsChange(object sender, MHRTrainingDojo e)
        {
            ViewModel.Boosts = e.Boosts;
            ViewModel.MaxBoosts = e.MaxBoosts;
        }

        private void OnBuddyNameChange(object sender, MHRBuddy e)
        {
            BuddyViewModel vm = _buddyViewModels[e];

            vm.Name = e.Name;
            vm.IsEmpty = string.IsNullOrEmpty(e.Name);
        }

        private void OnBuddyLevelChange(object sender, MHRBuddy e)
        {
            BuddyViewModel vm = _buddyViewModels[e];

            vm.Level = e.Level;
        }
    }
}
