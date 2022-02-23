using HunterPie.Core.Game.Enums;
using System;
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
                    
                    if (!part.IsPartBroken)
                        part.Health -= Math.Min(20, part.MaxHealth);

                    part.Flinch -= Math.Min(20, part.MaxFlinch);

                    if (!part.IsPartSevered)
                        part.Sever -= Math.Min(150, part.MaxSever);

                    if (part.Sever < 0)
                    {
                        part.Sever = part.MaxSever;
                        part.IsPartSevered = true;
                    }

                    if (part.Health < 0)
                    {
                        part.Health = part.MaxHealth;
                        part.IsPartBroken = true;
                    }

                    if (part.Flinch < 0)
                        part.Flinch = part.MaxFlinch;
                }
            };
            timer.Start();
        }

        private void MockParts()
        {
            for (int i = 0; i < 2; i++)
            {
                Parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Severable {i}",
                        Health = 500.0,
                        MaxHealth = 500.0,
                        Sever = 2000.0,
                        MaxSever = 2000.0,
                        Flinch = 200.0,
                        MaxFlinch = 200.0,
                        Breaks = 0,
                        MaxBreaks = 0,
                        IsPartBroken = (i & 1) == 1,
                        Type = PartType.Severable
                    }
                );
            }

            for (int i = 0; i < 2; i++)
            {
                Parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Breakable {i}",
                        Health = 500.0,
                        MaxHealth = 500.0,
                        Flinch = 250.0,
                        MaxFlinch = 250.0,
                        Breaks = 0,
                        MaxBreaks = 0,
                        IsPartBroken = (i & 1) == 1,
                        Type = PartType.Breakable
                    }
                );
            }

            for (int i = 0; i < 10; i++)
            {
                Parts.Add(
                    new MonsterPartViewModel()
                    {
                        Name = $"Part {i}",
                        Flinch = 250.0,
                        MaxFlinch = 250.0,
                        Breaks = 0,
                        MaxBreaks = 0,
                        Type = PartType.Invalid
                    }
                );
            }
        }

        private void MockAilments()
        {
            string[] ailmentNames =
            {
                "AILMENT_UNKNOWN", "AILMENT_PARALYSIS", "AILMENT_SLEEP", "AILMENT_STUN", "AILMENT_POISON",
                "AILMENT_BLAST", "AILMENT_EXHAUST", "AILMENT_MOUNT", "AILMENT_FLASH", "AILMENT_WATER", "AILMENT_FIRE",
                "AILMENT_ICE", "AILMENT_THUNDER", "STATUS_ENRAGE"
            };
            foreach (string name in ailmentNames)
            {
                Ailments.Add(
                    new MonsterAilmentViewModel()
                    {
                        Name = name,
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
