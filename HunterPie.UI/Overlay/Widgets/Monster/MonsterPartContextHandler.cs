using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterPartContextHandler : MonsterPartViewModel
    {
        public readonly IMonsterPart Context;

        public MonsterPartContextHandler(IMonsterPart context)
        {
            Context = context;
            HookEvents();
            Update();
        }

        private void HookEvents()
        {
            Context.OnHealthUpdate += OnHealthUpdate;
        }

        private void UnhookEvents()
        {
            Context.OnHealthUpdate -= OnHealthUpdate;
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

        private void Update()
        {
            Name = Context.Id;
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
        }
    }
}
