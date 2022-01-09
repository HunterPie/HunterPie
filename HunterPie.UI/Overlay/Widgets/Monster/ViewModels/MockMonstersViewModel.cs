using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MockMonstersViewModel : MonstersViewModel
    {
        public MockMonstersViewModel()
        {
            Monsters.Add(new MockBossMonsterViewModel() 
            {
                Name = "Monster",
                Em = "em116",
                MaxHealth = 35000,
                Health = 35000,
                Stamina = 10000,
                MaxStamina = 10000,
                Crown = Crown.Gold
            });
            Monsters.Add(new MockBossMonsterViewModel()
            {
                Name = "Monster",
                Em = "em120",
                MaxHealth = 15000,
                Health = 20000,
                Stamina = 1000,
                MaxStamina = 1000,
                Crown = Crown.Silver
            });
            Monsters.Add(new MockBossMonsterViewModel()
            {
                Name = "Monster",
                Em = "em103",
                MaxHealth = 35000,
                Health = 35000,
                Stamina = 10000,
                MaxStamina = 10000,
                Crown = Crown.None
            });
        }
    }
}
