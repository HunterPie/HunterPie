using HunterPie.UI.Architecture.Test;
using HunterPie.Core.Logger;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel
{
    public class MockAbnormalityBarViewModel : AbnormalityBarViewModel
    {
        private struct MockAbnormalityData
        {
            public string Name;
            public string Icon;
            public float MaxDuration;
            public bool IsDebuff;
        }
        private static MockAbnormalityData[] MockAbnormalityDatas =
        {
            // Songs
            new() { Name = "ABNORMALITY_SELF_IMPROVEMENT", Icon = "ICON_SELFIMPROVEMENT", MaxDuration = 180f },
            new() { Name = "ABNORMALITY_EARPLUG_PLUS", Icon = "ICON_EARPLUGS+", MaxDuration = 200f },
            new() { Name = "ABNORMALITY_STAMINA_USE_RED", Icon = "ICON_STAMINAUP", MaxDuration = 160f },
            new() { Name = "ABNORMALITY_INFERNAL_MELODY", Icon = "ICON_INFERNALMELODY", MaxDuration = 50f },
            
            // Consumables
            new() { Name = "ABNORMALITY_IMMUNITY", Icon = "ICON_IMMUNITY", MaxDuration = 250f },
            new() { Name = "ABNORMALITY_GO_FIGHT_WIN", Icon = "ICON_PALICO_YELLOW", MaxDuration = 120f },

            new() { Name = "ABNORMALITY_POISON", Icon = "ICON_POISON", MaxDuration = 60, IsDebuff = true }
        };

        public MockAbnormalityBarViewModel()
        {
            SetupAbnormalities();
            MockBehavior.Run(() =>
            {
                foreach (AbnormalityViewModel vm in Abnormalities)
                {
                    vm.Timer -= 1;

                    if (vm.Timer <= 0)
                        vm.Timer = vm.MaxTimer;
                }
            });
        }

        private void SetupAbnormalities()
        {
            foreach (MockAbnormalityData data in MockAbnormalityDatas)
            {
                var mock = new AbnormalityViewModel()
                {
                    Icon = data.Icon,
                    Name = data.Name,
                    Timer = data.MaxDuration,
                    MaxTimer = data.MaxDuration,
                    IsBuff = !data.IsDebuff
                };
                Abnormalities.Add(mock);
            }
        }

    }
}
