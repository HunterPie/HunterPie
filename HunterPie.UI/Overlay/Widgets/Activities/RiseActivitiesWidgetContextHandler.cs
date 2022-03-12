using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.Core.Game.Rise.Entities.Entity;
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
        private readonly ActivitiesViewModel ViewModel;

        private readonly Dictionary<MHRSubmarine, SubmarineViewModel> SubmarineViewModels;
        private readonly Dictionary<MHRBuddy, BuddyViewModel> TrainingBuddyViewModels;
        private TrainingDojoViewModel _trainingDojoViewModel = new();

        public RiseActivitiesWidgetContextHandler(MHRContext context)
        {
            _context = context;
            _player = (MHRPlayer)context.Game.Player;

            var widget = new ActivitiesView();
            WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(widget);

            ViewModel = widget.ViewModel;
            
            SubmarineViewModels = new(_player.Argosy.Submarines.Length);
            TrainingBuddyViewModels = new(_player.TrainingDojo.Buddies.Length);

            UpdateData();
            HookEvents();
        }

        private void HookEvents()
        {
            SubmarinesViewModel submarinesViewModel = new();

            foreach (MHRSubmarine submarine in _player.Argosy.Submarines)
            {
                SubmarineViewModels.Add(submarine, new SubmarineViewModel() { 
                    Count = submarine.Count,
                    MaxCount = submarine.MaxCount,
                    IsActive = submarine.IsUnlocked,
                    DaysLeft = submarine.DaysLeft
                });
                submarine.OnItemCountChange += OnSubmarineItemCountChange;
                submarine.OnDaysLeftChange += OnSubmarineDaysLeftChange;
                submarine.OnLockStateChange += OnSubmarineLockStateChange;
            }

            _trainingDojoViewModel.Rounds = _player.TrainingDojo.Rounds;
            _trainingDojoViewModel.MaxRounds = _player.TrainingDojo.MaxRounds;
            _trainingDojoViewModel.Boosts = _player.TrainingDojo.Boosts;
            _trainingDojoViewModel.MaxBoosts = _player.TrainingDojo.MaxBoosts;

            foreach (MHRBuddy buddy in _player.TrainingDojo.Buddies)
            {
                TrainingBuddyViewModels.Add(buddy, new BuddyViewModel()
                {
                    Name = buddy.Name,
                    Level = buddy.Level,
                    IsEmpty = string.IsNullOrEmpty(buddy.Name)
                });
                buddy.OnNameChange += OnBuddyNameChange;
                buddy.OnLevelChange += OnBuddyLevelChange;
            }

            _player.TrainingDojo.OnBoostsLeftChange += OnTrainingBoostChange;
            _player.TrainingDojo.OnRoundsLeftChange += OnTrainingRoundsChange;

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                foreach (SubmarineViewModel vm in SubmarineViewModels.Values)
                    submarinesViewModel.Submarines.Add(vm);

                foreach (BuddyViewModel vm in TrainingBuddyViewModels.Values)
                    _trainingDojoViewModel.Buddies.Add(vm);
            });

            ViewModel.Activities.Add(submarinesViewModel);
            ViewModel.Activities.Add(_trainingDojoViewModel);

            _player.OnStageUpdate += OnStageChange;
        }

        private void OnTrainingRoundsChange(object sender, MHRTrainingDojo e)
        {
            _trainingDojoViewModel.MaxRounds = e.MaxRounds;
            _trainingDojoViewModel.Rounds = e.Rounds;
        }

        private void OnTrainingBoostChange(object sender, MHRTrainingDojo e)
        {
            _trainingDojoViewModel.MaxBoosts = e.MaxBoosts;
            _trainingDojoViewModel.Boosts = e.Boosts;
        }

        private void UpdateData()
        {
            ViewModel.InVisibleStage = MHRGame.VillageStages.Contains(_player.StageId);
        }

        private void OnStageChange(object sender, EventArgs e)
        {
            ViewModel.InVisibleStage = MHRGame.VillageStages.Contains(_player.StageId);
        }

        private void OnBuddyLevelChange(object sender, MHRBuddy e)
        {
            BuddyViewModel vm = TrainingBuddyViewModels[e];
            vm.Level = e.Level;
        }

        private void OnBuddyNameChange(object sender, MHRBuddy e)
        {
            BuddyViewModel vm = TrainingBuddyViewModels[e];
            vm.Name = e.Name;
            vm.IsEmpty = string.IsNullOrEmpty(e.Name);
        }

        private void OnSubmarineLockStateChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = SubmarineViewModels[e];
            
            vm.IsActive = e.IsUnlocked;
        }

        private void OnSubmarineDaysLeftChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = SubmarineViewModels[e];

            vm.DaysLeft = e.DaysLeft;
        }

        private void OnSubmarineItemCountChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = SubmarineViewModels[e];

            vm.Count = e.Count;
            vm.MaxCount = e.MaxCount;
        }

        public void UnhookEvents()
        {
            foreach (var submarine in SubmarineViewModels.Keys)
            {
                submarine.OnItemCountChange -= OnSubmarineItemCountChange;
                submarine.OnDaysLeftChange -= OnSubmarineDaysLeftChange;
                submarine.OnLockStateChange -= OnSubmarineLockStateChange;
            }

            _player.TrainingDojo.OnBoostsLeftChange -= OnTrainingBoostChange;
            _player.TrainingDojo.OnRoundsLeftChange -= OnTrainingRoundsChange;
            foreach (var buddy in TrainingBuddyViewModels.Keys)
            {
                buddy.OnNameChange -= OnBuddyNameChange;
                buddy.OnLevelChange -= OnBuddyLevelChange;
            }

            SubmarineViewModels.Clear();
            TrainingBuddyViewModels.Clear();

            _player.OnStageUpdate -= OnStageChange;
        }

        
    }
}
