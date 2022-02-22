using HunterPie.Core.Game.Rise.Entities;
using HunterPie.UI.Overlay.Widgets.Wirebug.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Wirebug
{
    class WirebugContextHandler : WirebugViewModel, IContextHandler
    {

        public readonly MHRWirebug Context;

        public WirebugContextHandler(MHRWirebug context)
        {
            Context = context;

            UpdateData();
            HookEvents();
        }

        private void HookEvents()
        {
            Context.OnCooldownUpdate += OnCooldownUpdate;
            Context.OnTimerUpdate += OnTimerUpdate;
            Context.OnAvailable += OnAvailable;
        }

        public void UnhookEvents()
        {
            Context.OnCooldownUpdate -= OnCooldownUpdate;
            Context.OnAvailable -= OnAvailable;
        }

        private void OnTimerUpdate(object sender, MHRWirebug e)
        {
            MaxTimer = e.MaxTimer;
            Timer = e.Timer;
        }

        private void OnAvailable(object sender, MHRWirebug e)
        {
            IsAvailable = e.IsAvailable;
        }

        private void OnCooldownUpdate(object sender, MHRWirebug e)
        {
            MaxCooldown = e.MaxCooldown;
            Cooldown = e.Cooldown;
            OnCooldown = Cooldown > 0;
        }

        private void UpdateData()
        {
            MaxCooldown = Context.MaxCooldown;
            Cooldown = Context.Cooldown;
            IsAvailable = Context.IsAvailable;
        }
    }
}
