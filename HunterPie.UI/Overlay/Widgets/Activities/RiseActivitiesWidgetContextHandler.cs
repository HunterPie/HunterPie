using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.UI.Overlay.Widgets.Activities.View;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System.Collections.Generic;
using System.Windows.Threading;

namespace HunterPie.UI.Overlay.Widgets.Activities
{
    public class RiseActivitiesWidgetContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly MHRPlayer _player;
        private readonly ActivitiesViewModel ViewModel;

        private readonly Dictionary<MHRSubmarine, SubmarineViewModel> SubmarineViewModels;

        public RiseActivitiesWidgetContextHandler(MHRContext context)
        {
            _context = context;
            _player = (MHRPlayer)context.Game.Player;

            var widget = new ActivitiesView();
            WidgetManager.Register<ActivitiesView, ActivitiesWidgetConfig>(widget);

            ViewModel = widget.ViewModel;
            
            SubmarineViewModels = new(_player.Argosy.Submarines.Length);

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

            Dispatcher.CurrentDispatcher.Invoke(() =>
            {
                foreach (SubmarineViewModel vm in SubmarineViewModels.Values)
                    submarinesViewModel.Submarines.Add(vm);
            });

            ViewModel.Activities.Add(submarinesViewModel);
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

            SubmarineViewModels.Clear();
        }

        
    }
}
