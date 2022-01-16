using HunterPie.Core.Game;
using HunterPie.Core.Game.Environment;
using HunterPie.UI.Overlay.Widgets.Monster.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        private void OnSpawn(object sender, EventArgs e)
        {
            Name = Context.Name;
            Em = $"Rise_{Context.Id:00}";
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
            }
            MaxHealth = Context.MaxHealth;
            Health = Context.Health;
            MaxStamina = 0;
            Stamina = 0;
        }
    }
}
