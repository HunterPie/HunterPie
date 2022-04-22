using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.World.Entities;
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
            Context.OnStaminaChange += OnStaminaUpdate;
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
            Context.OnStaminaChange -= OnStaminaUpdate;
            Context.OnEnrageStateChange -= OnEnrageStateChange;
            Context.OnSpawn -= OnSpawn;
            Context.OnDeath -= OnDespawn;
            Context.OnDespawn -= OnDespawn;
            Context.OnTargetChange -= OnTargetChange;
            Context.OnNewPartFound -= OnNewPartFound;
            Context.OnNewAilmentFound -= OnNewAilmentFound;
            Context.OnCrownChange -= OnCrownChange;
        }
        
        private void OnSpawn(object sender, EventArgs e)
        {
            Name = Context.Name;

            Em = BuildMonsterEmByContext();

            IsAlive = true;

            FetchMonsterIcon();
        }

        private void OnDespawn(object sender, EventArgs e)
        {
            UnhookEvents();
            IsAlive = false;
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

        private void OnStaminaUpdate(object sender, EventArgs e)
        {
            MaxStamina = Context.MaxStamina;
            Stamina = Context.Stamina;
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
            IsTarget = Context.Target == Target.Self || (Context.Target == Target.None && !Config.ShowOnlyTarget);
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
                Em = BuildMonsterEmByContext();

                FetchMonsterIcon();
            }
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
            IsTarget = Context.Target == Target.Self || (Context.Target == Target.None && !Config.ShowOnlyTarget);
            MaxStamina = Context.MaxStamina;
            Stamina = Context.Stamina;
            TargetType = Context.Target;
            Crown = Context.Crown;
            IsEnraged = Context.IsEnraged;
            IsAlive = Context.Health > 0;
            
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                Ailments.Add(new MonsterAilmentContextHandler(Context.Enrage));
            });
        }

        private string BuildMonsterEmByContext()
        {
            return Context switch
            {
                MHRMonster ctx => $"Rise_{ctx.Id:00}",
                MHWMonster ctx => $"World_{ctx.Id:00}",
                _ => throw new NotImplementedException("unreachable")
            };
        }
    }
}
