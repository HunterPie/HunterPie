using HunterPie.Core.Game.Enums;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    public class MockMonstersViewModel : MonstersViewModel
    {
        public MockMonstersViewModel()
        {
            Monsters.Add(new MockBossMonsterViewModel() 
            {
                Name = "Monster",
                Em = "Rise_32",
                MaxHealth = 35000,
                Health = 35000,
                Stamina = 10000,
                MaxStamina = 10000,
                Crown = Crown.Gold,
                TargetType = Target.Self,
                IsTarget = true,
            });
            
            foreach (BossMonsterViewModel vm in Monsters)
                vm.FetchMonsterIcon();
        }
    }
}
