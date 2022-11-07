using WeaponId = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.UI.Overlay.Widgets.Player.ViewModels;

public class MockPlayerHudViewModel : PlayerHudViewModel
{

    public MockPlayerHudViewModel()
    {
        SetupMocks();
    }

    private void SetupMocks()
    {
        Name = "Lyss";
        Level = 99;
        Health = 180.0;
        MaxHealth = 180.0;
        MaxExtendableHealth = 200.0;
        RecoverableHealth = 0;
        Stamina = 2000.0;
        MaxStamina = 2500.0;
        Weapon = WeaponId.SwitchAxe;
    }
}
