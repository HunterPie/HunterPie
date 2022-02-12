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
            Context.OnFlinchUpdate += OnFlinchUpdate;
        }

        private void OnFlinchUpdate(object sender, IMonsterPart e)
        {
            // In case this part has only flinch values, we can display it in the main health bar
            if (e.MaxHealth < 0)
            {
                MaxHealth = e.MaxFlinch;
                Health = e.Flinch;
                return;
            }
            Tenderize = e.Flinch;
            MaxTenderize = e.MaxFlinch;
        }

        private void UnhookEvents()
        {
            Context.OnHealthUpdate -= OnHealthUpdate;
            Context.OnFlinchUpdate -= OnFlinchUpdate;
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
            if (Context.MaxHealth > 0)
            {
                MaxHealth = Context.MaxHealth;
                Health = Context.Health;
                Tenderize = Context.Flinch;
                MaxTenderize = Context.MaxFlinch;
            } else
            {
                MaxHealth = Context.MaxFlinch;
                Health = Context.Flinch;
            }
            
        }
    }
}
