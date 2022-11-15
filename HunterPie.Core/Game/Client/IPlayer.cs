using HunterPie.Core.Game.Events;
using System;
using System.Collections.Generic;

namespace HunterPie.Core.Game.Client;

public interface IPlayer
{
    public string Name { get; }
    public int HighRank { get; }
    public int MasterRank { get; }
    public int StageId { get; }
    public bool InHuntingZone { get; }
    public IParty Party { get; }
    public IReadOnlyCollection<IAbnormality> Abnormalities { get; }
    public double Health { get; }
    public double MaxHealth { get; }
    public double RecoverableHealth { get; }
    public double MaxPossibleHealth { get; }
    public double Heal { get; }
    public double Stamina { get; }
    public double MaxStamina { get; }
    public double MaxRecoverableStamina { get; }
    public double MaxPossibleStamina { get; }
    public IWeapon Weapon { get; }


    public event EventHandler<EventArgs> OnLogin;
    public event EventHandler<EventArgs> OnLogout;
    public event EventHandler<EventArgs> OnDeath;
    public event EventHandler<EventArgs> OnActionUpdate;
    public event EventHandler<EventArgs> OnStageUpdate;
    public event EventHandler<EventArgs> OnVillageEnter;
    public event EventHandler<EventArgs> OnVillageLeave;
    public event EventHandler<EventArgs> OnAilmentUpdate;
    public event EventHandler<WeaponChangeEventArgs> OnWeaponChange;
    public event EventHandler<IAbnormality> OnAbnormalityStart;
    public event EventHandler<IAbnormality> OnAbnormalityEnd;
    public event EventHandler<HealthChangeEventArgs> OnHeal;
    public event EventHandler<HealthChangeEventArgs> OnHealthChange;
    public event EventHandler<StaminaChangeEventArgs> OnStaminaChange;
    public event EventHandler<LevelChangeEventArgs> OnLevelChange;
}
