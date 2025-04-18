﻿using HunterPie.Core.Client.Configuration.Overlay;
using HunterPie.Core.Game.Entity.Enemy;
using HunterPie.Core.Game.Enums;
using System;
using System.Linq;

namespace HunterPie.UI.Overlay.Widgets.Monster.ViewModels;

public class MockMonstersViewModel : MonstersViewModel
{
    private bool _isRandomMonsterSelected;
    public bool IsRandomMonsterSelected
    {
        get => _isRandomMonsterSelected;
        set
        {
            Monster = value
                ? Monsters.ElementAt((int)Random.Shared.NextInt64(0, Monsters.Count))
                : null;

            SetValue(ref _isRandomMonsterSelected, value);
        }
    }

    public MockMonstersViewModel(MonsterWidgetConfig config)
    {
        Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000,
            Stamina = 10000,
            MaxStamina = 10000,
            Crown = Crown.Gold,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.2,
            Variant = VariantType.Normal
        });
        Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster 2",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000,
            Stamina = 10000,
            MaxStamina = 10000,
            Crown = Crown.Silver,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.25,
            Variant = VariantType.Tempered
        });
        Monsters.Add(new MockBossMonsterViewModel(config)
        {
            Name = "Monster 3",
            Em = "Rise_32",
            MaxHealth = 35000,
            Health = 35000 * 0.53,
            Stamina = 8500,
            MaxStamina = 10000,
            Crown = Crown.Mini,
            TargetType = Target.None,
            IsTarget = false,
            IsAlive = true,
            CaptureThreshold = 0.5,
            IsCapturable = true,
            Variant = VariantType.Tempered | VariantType.Frenzy
        });
        VisibleMonsters = 3;
        MonstersCount = 3;

        foreach (BossMonsterViewModel vm in Monsters)
            vm.FetchMonsterIcon();
    }
}