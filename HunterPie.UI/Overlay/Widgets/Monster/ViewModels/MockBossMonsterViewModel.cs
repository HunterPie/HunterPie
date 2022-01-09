using HunterPie.Core.Architecture;
using HunterPie.Core.Game.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    internal class MockBossMonsterViewModel : BossMonsterViewModel
    {
        public MockBossMonsterViewModel()
        {
            MockParts();
            MockAilments();
        }

        private void MockParts()
        {
            for (int i = 0; i < 12; i++)
            {
                Parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Part {i}",
                        Health = 200.0,
                        MaxHealth = 250.0,
                        Tenderize = 10.0,
                        MaxTenderize = 10.0,
                        Break = 0,
                        MaxBreaks = 0
                    }
                );
            }
        }

        private void MockAilments()
        {
            for (int i = 0; i < 19; i++)
            {
                Ailments.Add(
                    new MonsterAilmentViewModel()
                    {
                        Name = "Ailment",
                        Timer = 100.0,
                        MaxTimer = 100.0,
                        Buildup = 100.0,
                        MaxBuildup = 100.0
                    }
                );
            }
        }
    }
}
