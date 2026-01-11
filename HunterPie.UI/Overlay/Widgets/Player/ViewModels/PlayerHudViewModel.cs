using HunterPie.Core.Architecture.Collections;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Player.ViewModels;

public class PlayerHudViewModel(IWidgetSettings settings) : WidgetViewModel(settings, "Player Hud Widget", WidgetType.ClickThrough)
{
    public int Level { get; set => SetValue(ref field, value); }
    public string Name { get; set => SetValue(ref field, value); }
    public double Health { get; set => SetValue(ref field, value); }
    public double MaxHealth { get; set => SetValue(ref field, value); }
    public double MaxExtendableHealth { get; set => SetValue(ref field, value); }
    public double RecoverableHealth { get; set => SetValue(ref field, value); }
    public double Heal { get; set => SetValue(ref field, value); }
    public double Stamina { get; set => SetValue(ref field, value); }
    public double MaxStamina { get; set => SetValue(ref field, value); }
    public double MaxPossibleStamina { get; set => SetValue(ref field, value); }
    public double MaxRecoverableStamina { get; set => SetValue(ref field, value); }
    public bool InHuntingZone { get; set => SetValue(ref field, value); }
    public Weapon Weapon { get; set => SetValue(ref field, value); }

    public ThreadSafeObservableCollection<AbnormalityCategory> ActiveAbnormalities { get; } = new();
    public bool IsMoxieActive { get; set => SetValue(ref field, value); }

    public WeaponSharpnessViewModel SharpnessViewModel { get; } = new();
}