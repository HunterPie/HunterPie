using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
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
            UpdateData();
        }

        private void HookEvents()
        {
            Context.OnHealthChange += OnHealthUpdate;
            Context.OnSpawn += OnSpawn;
            Context.OnDeath += OnDespawn;
            Context.OnDespawn += OnDespawn;
            Context.OnTargetChange += OnTargetChange;
        }

        private void UnhookEvents()
        {
            Context.OnHealthChange -= OnHealthUpdate;
            Context.OnSpawn -= OnSpawn;
            Context.OnDeath -= OnDespawn;
            Context.OnDespawn -= OnDespawn;
            Context.OnTargetChange -= OnTargetChange;
        }
        
        private void OnSpawn(object sender, EventArgs e)
        {
            Name = Context.Name;
            Em = $"Rise_{Context.Id:00}";
        }

        private void OnDespawn(object sender, EventArgs e)
        {
            UnhookEvents();

            Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (MonsterPartContextHandler part in Parts)
                    part.Dispose();

                Parts.Clear();
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

            if (Parts.Count != Context.Parts.Length)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (IMonsterPart part in Context.Parts)
                        Parts.Add(new MonsterPartContextHandler(part) { Name = "Unknown", Health = part.Health, MaxHealth = part.MaxHealth });
                });
            }
        }

        private void UpdateData()
        {
            if (Context.Id > -1)
            {
                Name = Context.Name;
                Em = $"Rise_{Context.Id:00}";
            }
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
            IsTarget = Context.Target == Target.Self || Context.Target == Target.None;
            MaxStamina = 1;
            Stamina = 1;
            TargetType = Context.Target;

            if (Parts.Count != Context.Parts.Length)
            {
                Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    foreach (IMonsterPart part in Context.Parts)
                        Parts.Add(new MonsterPartContextHandler(part) { Name = "Unknown", Health = part.Health, MaxHealth = part.MaxHealth });
                });
            }
        }
    }
}
