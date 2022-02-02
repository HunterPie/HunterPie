using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterAilmentContextHandler: MonsterAilmentViewModel
    {

        public readonly IMonsterAilment Context;

        public MonsterAilmentContextHandler(IMonsterAilment context)
        {
            Context = context;

            Update();
            HookEvents();
        }

        private void HookEvents()
        {
            Context.OnTimerUpdate += OnTimerUpdate;
            Context.OnBuildUpUpdate += OnBuildUpUpdate;
            Context.OnCounterUpdate += OnCounterUpdate;
        }

        private void UnhookEvents()
        {
            Context.OnTimerUpdate -= OnTimerUpdate;
            Context.OnBuildUpUpdate -= OnBuildUpUpdate;
            Context.OnCounterUpdate -= OnCounterUpdate;
        }

        private void OnCounterUpdate(object sender, IMonsterAilment e)
        {
            Count = e.Counter;
        }

        private void OnBuildUpUpdate(object sender, IMonsterAilment e)
        {
            MaxBuildup = e.MaxBuildUp;
            Buildup = e.BuildUp;
        }

        private void OnTimerUpdate(object sender, IMonsterAilment e)
        {
            MaxTimer = e.MaxTimer;
            Timer = e.Timer;
        }

        private void Update()
        {
            Name = Context.Id;
            Count = Context.Counter;
            MaxBuildup = Context.MaxBuildUp;
            Buildup = Context.BuildUp;
            MaxTimer = Context.MaxTimer;
            Timer = Context.Timer;
        }

        protected override void DisposeResources()
        {
            base.DisposeResources();
            UnhookEvents();
        }
    }
}
