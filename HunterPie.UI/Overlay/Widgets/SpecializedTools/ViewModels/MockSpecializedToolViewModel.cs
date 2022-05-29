using HunterPie.Core.Game.World.Entities;
using HunterPie.UI.Architecture.Test;

namespace HunterPie.UI.Overlay.Widgets.SpecializedTools.ViewModels
{
    public class MockSpecializedToolViewModel : SpecializedToolViewModel
    {
        const double MAX_TIMER = 20;
        const double MAX_COOLDOWN = 20;

        public MockSpecializedToolViewModel()
        {
            Id = MHWSpecializedTool.ChallengerMantle;
            MaxCooldown = MAX_COOLDOWN;
            Timer = MaxTimer = MAX_TIMER;

            MockBehavior.Run(() =>
            {
                
                if (IsRecharging)
                {
                    Cooldown++;
                    Timer = 120;
                    IsRecharging = Cooldown != MaxCooldown;
                } else
                {
                    Timer--;
                    Cooldown = 0;
                    IsRecharging = Timer <= 0;
                }
            });
        }

    }
}
