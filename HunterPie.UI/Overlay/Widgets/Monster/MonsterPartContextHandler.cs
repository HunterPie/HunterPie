using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterPartContextHandler : MonsterPartViewModel
    {
        public readonly IMonsterPart Context;

        public MonsterPartContextHandler(IMonsterPart context, MonsterWidgetConfig config) : base(config)
        {
            Context = context;
            Type = Context.Type;

            HookEvents();
            Update();
        }

        private void HookEvents()
        {
            Context.OnHealthUpdate += OnHealthUpdate;
            Context.OnFlinchUpdate += OnFlinchUpdate;
            Context.OnSeverUpdate += OnSeverUpdate;
        }

        private void OnSeverUpdate(object sender, IMonsterPart e)
        {
            MaxSever = e.MaxSever;
            Sever = e.Sever;

            IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
        }

        private void OnFlinchUpdate(object sender, IMonsterPart e)
        {
            if (Flinch < e.Flinch && MaxFlinch > 0)
                Breaks++;

            MaxFlinch = e.MaxFlinch;
            Flinch = e.Flinch;

            IsPartBroken = MaxHealth <= 0 || Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch);
            IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
        }

        private void UnhookEvents()
        {
            Context.OnHealthUpdate -= OnHealthUpdate;
            Context.OnFlinchUpdate -= OnFlinchUpdate;
            Context.OnSeverUpdate -= OnSeverUpdate;
        }

        private void OnHealthUpdate(object sender, IMonsterPart e)
        {
            MaxHealth = e.MaxHealth;
            Health = e.Health;

            IsPartBroken = MaxHealth <= 0 || Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch);
        }

        protected override void DisposeResources()
        {
            base.DisposeResources();
            UnhookEvents();
        }

        private void Update()
        {
            Name = Context.Id;
            IsKnownPart = Context.Id != "PART_UNKNOWN";

            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
            MaxFlinch = Context.MaxFlinch;
            Flinch = Context.Flinch;
            MaxSever = Context.MaxSever;
            Sever = Context.Sever;

            IsPartSevered = MaxSever == Sever && (Breaks > 0 || Flinch != MaxFlinch);
            IsPartBroken = MaxHealth <= 0 || (Health == MaxHealth && (Breaks > 0 || Flinch != MaxFlinch));

        }
    }
}
