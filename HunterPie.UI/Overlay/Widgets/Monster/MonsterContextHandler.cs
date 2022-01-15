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
        private readonly IMonster Context;

        public MonsterContextHandler(IMonster context) : base()
        {
            Context = context;
            HookEvents();
            UpdateData();
        }

        private void HookEvents()
        {
            Context.OnHealthChange += OnHealthUpdate;
        }

        private void OnHealthUpdate(object sender, EventArgs e)
        {
            MaxHealth = Math.Max(Context.Health, MaxHealth);
            Health = Context.Health;
        }

        private void UpdateData()
        {
            Name = "Unknown";
            MaxHealth = Math.Max(Context.Health, MaxHealth);
            Health = Context.Health;
            MaxStamina = 0;
            Stamina = 0;
        }
    }
}
