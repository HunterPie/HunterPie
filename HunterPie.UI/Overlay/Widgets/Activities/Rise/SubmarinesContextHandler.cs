using HunterPie.Core.Game.Rise;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Activities;
using HunterPie.UI.Overlay.Widgets.Activities.ViewModel;
using System.Collections.Generic;

namespace HunterPie.UI.Overlay.Widgets.Activities.Rise
{
    internal class SubmarinesContextHandler : IContextHandler
    {
        private readonly MHRContext _context;
        private readonly Dictionary<MHRSubmarine, SubmarineViewModel> _submarineViewModels;
        private MHRPlayer _player => (MHRPlayer)_context.Game.Player;

        public readonly SubmarinesViewModel ViewModel = new();

        public SubmarinesContextHandler(MHRContext context)
        {
            _context = context;
            _submarineViewModels = new(_player.Argosy.Submarines.Length);
        }

        public void HookEvents()
        {
            foreach (MHRSubmarine submarine in _player.Argosy.Submarines)
            {
                if (!_submarineViewModels.ContainsKey(submarine))
                    _submarineViewModels[submarine] = new()
                    {
                        Count = submarine.Count,
                        MaxCount = submarine.MaxCount,
                        DaysLeft = submarine.DaysLeft,
                        IsActive = submarine.IsUnlocked
                    };

                submarine.OnDaysLeftChange += OnDaysLeftChange;
                submarine.OnItemCountChange += OnItemCountChange;
                submarine.OnLockStateChange += OnLockStateChange;
            }

            foreach (SubmarineViewModel vm in _submarineViewModels.Values)
                ViewModel.Submarines.Add(vm);
        }

        public void UnhookEvents()
        {
            foreach (MHRSubmarine submarine in _submarineViewModels.Keys)
            {
                submarine.OnDaysLeftChange -= OnDaysLeftChange;
                submarine.OnItemCountChange -= OnItemCountChange;
                submarine.OnLockStateChange -= OnLockStateChange;
            }

            _submarineViewModels.Clear();
            ViewModel.Submarines.Clear();
        }

        private void OnLockStateChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = _submarineViewModels[e];

            vm.IsActive = e.IsUnlocked;
        }

        private void OnItemCountChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = _submarineViewModels[e];

            vm.Count = e.Count;
            vm.MaxCount = e.MaxCount;
        }

        private void OnDaysLeftChange(object sender, MHRSubmarine e)
        {
            SubmarineViewModel vm = _submarineViewModels[e];

            vm.DaysLeft = e.DaysLeft;
        }
    }
}
