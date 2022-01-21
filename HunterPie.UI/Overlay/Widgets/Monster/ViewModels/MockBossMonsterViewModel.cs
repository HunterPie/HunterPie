using HunterPie.Core.Game.Enums;
using System.Timers;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels
{
    internal class MockBossMonsterViewModel : BossMonsterViewModel
    {
        private Timer timer = new(1000);

        public MockBossMonsterViewModel()
        {
            MockParts();
            MockAilments();
            timer.Elapsed += (_, __) =>
            {
                for (int i = 0; i < Parts.Count / 2; i++)
                {
                    var part = Parts[i];
                    part.Health -= 20;

                    if (part.Health < 0)
                        part.Health = part.MaxHealth;
                }
            };
            timer.Start(); 
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
                        Breaks = 0,
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
