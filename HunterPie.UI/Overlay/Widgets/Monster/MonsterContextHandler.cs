using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace HunterPie.UI.Overlay.Widgets.Monster
{
    public class MonsterContextHandler : BossMonsterViewModel
    {
        public readonly IMonster Context;

        public MonsterContextHandler(IMonster context) : base()
        {
            Context = context;
            HookEvents();

            AddEnrage();
            UpdateData();
        }

        private void HookEvents()
        {
            Context.OnHealthChange += OnHealthUpdate;
            Context.OnEnrageStateChange += OnEnrageStateChange;
            Context.OnSpawn += OnSpawn;
            Context.OnDeath += OnDespawn;
            Context.OnDespawn += OnDespawn;
            Context.OnTargetChange += OnTargetChange;
            Context.OnNewPartFound += OnNewPartFound;
            Context.OnNewAilmentFound += OnNewAilmentFound;
            Context.OnCrownChange += OnCrownChange;
        }

        private void UnhookEvents()
        {
            Context.OnHealthChange -= OnHealthUpdate;
            Context.OnEnrageStateChange -= OnEnrageStateChange;
            Context.OnSpawn -= OnSpawn;
            Context.OnDeath -= OnDespawn;
            Context.OnDespawn -= OnDespawn;
            Context.OnTargetChange -= OnTargetChange;
            Context.OnNewPartFound -= OnNewPartFound;
            Context.OnCrownChange -= OnCrownChange;
        }
        
        private void OnSpawn(object sender, EventArgs e)
        {
            Name = Context.Name;
            Em = $"Rise_{Context.Id:00}";

            FetchMonsterIcon();
        }

        private void OnDespawn(object sender, EventArgs e)
        {
            UnhookEvents();

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (MonsterPartContextHandler part in Parts)
                    part.Dispose();

                foreach (MonsterAilmentContextHandler ailment in Ailments)
                    ailment.Dispose();

                Parts.Clear();
                Ailments.Clear();
            });
        }

        private void OnEnrageStateChange(object sender, EventArgs e) => IsEnraged = Context.IsEnraged;

        private void OnCrownChange(object sender, EventArgs e)
        {
            Crown = Context.Crown;
        }

        private void OnNewAilmentFound(object sender, IMonsterAilment e)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                bool contains = Ailments.ToArray()
                            .Cast<MonsterAilmentContextHandler>()
                            .Where(p => p.Context == e).Count() > 0;

                if (contains)
                    return;

                Ailments.Add(new MonsterAilmentContextHandler(e));
            });
        }

        private void OnNewPartFound(object sender, IMonsterPart e)
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                bool contains = Parts.ToArray()
                            .Cast<MonsterPartContextHandler>()
                            .Where(p => p.Context == e).Count() > 0;

                if (contains)
                    return;

                Parts.Add(new MonsterPartContextHandler(e));
            });
        }

        private void OnTargetChange(object sender, EventArgs e) 
        {
            IsTarget = Context.Target == Target.Self || Context.Target == Target.None;
            TargetType = Context.Target;
        }

        private void OnHealthUpdate(object sender, EventArgs e)
        {
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
        }

        private void UpdateData()
        {
            if (Context.Id > -1)
            {
                Name = Context.Name;
                Em = $"Rise_{Context.Id:00}";

                FetchMonsterIcon();
            }
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
            IsTarget = Context.Target == Target.Self || Context.Target == Target.None;
            MaxStamina = 1;
            Stamina = 1;
            TargetType = Context.Target;
            Crown = Context.Crown;
            IsEnraged = Context.IsEnraged;
            
            if (Parts.Count != Context.Parts.Length || Ailments.Count != Context.Ailments.Length)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (IMonsterPart part in Context.Parts)
                    {
                        bool contains = Parts.ToArray()
                            .Cast<MonsterPartContextHandler>()
                            .Where(p => p.Context == part).Count() > 0;

                        if (contains)
                            continue;

                        Parts.Add(new MonsterPartContextHandler(part));
                    }

                    foreach (IMonsterAilment ailment in Context.Ailments)
                    {
                        bool contains = Ailments.ToArray()
                            .Cast<MonsterAilmentContextHandler>()
                            .Where(p => p.Context == ailment).Count() > 0;

                        if (contains)
                            continue;

                        Ailments.Add(new MonsterAilmentContextHandler(ailment));
                    }
                });
            }
        }

        private void AddEnrage()
        {
            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Ailments.Add(new MonsterAilmentContextHandler(Context.Enrage));
            });
        }
    }
}
