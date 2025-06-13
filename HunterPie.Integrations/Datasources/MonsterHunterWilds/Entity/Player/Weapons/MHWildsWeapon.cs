﻿using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Weapons;

public class MHWildsWeapon : IWeapon
{
    public Weapon Id { get; }

    public MHWildsWeapon(Weapon id)
    {
        Id = id;
    }
}