using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterPartContextHandler : MonsterPartViewModel, IDisposable
    {
        private readonly IMonsterPart _context;

        public MonsterPartContextHandler(IMonsterPart context)
        {
            _context = context;
            HookEvents();
        }

        private void HookEvents()
        {
            _context.OnHealthUpdate += OnHealthUpdate;
        }

        private void UnhookEvents()
        {
            _context.OnHealthUpdate -= OnHealthUpdate;
        }

        private void OnHealthUpdate(object sender, IMonsterPart e)
        {
            MaxHealth = e.MaxHealth;
            Health = e.Health;
        }

        protected override void DisposeResources()
        {
            base.DisposeResources();
            UnhookEvents();
        }
    }
}
