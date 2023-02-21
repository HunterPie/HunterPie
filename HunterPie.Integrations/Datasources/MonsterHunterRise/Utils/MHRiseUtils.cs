﻿using HunterPie.Core.Domain.Memory;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using System.Runtime.CompilerServices;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;

public static class MHRiseUtils
{
    private readonly static HashSet<QuestType> HuntQuestTypes = new() { QuestType.Normal, QuestType.Kill, QuestType.Capture, QuestType.Arena };
    private const double MAX_DEFAULT_STAMINA = 1500.0;
    private const double FOOD_BONUS_STAMINA = 3000.0;
    private const double MAX_DEFAULT_HEALTH = 100.0;
    private const double FOOD_BONUS_HEALTH = 50.0;
    private const double PETALACE_STAMINA_MULTIPLIER = 30.0;

    public static bool IsHuntQuest(this QuestType type) => HuntQuestTypes.Contains(type);

    public static Weapon ToWeaponId(this int self)
    {
        return self switch
        {
            0 => Weapon.Greatsword,
            1 => Weapon.SwitchAxe,
            2 => Weapon.Longsword,
            3 => Weapon.LightBowgun,
            4 => Weapon.HeavyBowgun,
            5 => Weapon.Hammer,
            6 => Weapon.GunLance,
            7 => Weapon.Lance,
            8 => Weapon.SwordAndShield,
            9 => Weapon.DualBlades,
            10 => Weapon.HuntingHorn,
            11 => Weapon.ChargeBlade,
            12 => Weapon.InsectGlaive,
            13 => Weapon.Bow,
            _ => Weapon.None,
        };
    }

    public static bool IsInQuest(this int self) => self == 2;
    public static bool IsQuestFinished(this int self) => self > 2;
    public static bool IsTrainingRoom(this int self) => self == 5;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CalculateMaxPlayerHealth(this MHRPetalaceStatsStructure stats) =>
        stats.HealthUp + MAX_DEFAULT_HEALTH + FOOD_BONUS_HEALTH;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CalculateMaxPlayerStamina(this MHRPetalaceStatsStructure stats) =>
        (stats.StaminaUp * PETALACE_STAMINA_MULTIPLIER) + MAX_DEFAULT_STAMINA + FOOD_BONUS_STAMINA;

    public static T[] ReadArray<T>(this IMemory memory, long address) where T : struct
    {
        uint arraySize = memory.Read<uint>(address + 0x1C);
        return memory.Read<T>(address + 0x20, arraySize);
    }

    public static T[] ReadArrayOfPtrs<T>(this IMemory memory, long address) where T : struct
    {
        return memory.ReadArray<long>(address)
            .Select(memory.Read<T>)
            .ToArray();
    }

    public static float ReadEncryptedFloat(this IMemory memory, long encryptedFloatPtr)
    {
        MHRCryptoFloatStructure structure = memory.Read<MHRCryptoFloatStructure>(encryptedFloatPtr + 0x14);

        return MHRCrypto.DecodeHealth(structure.GetValue(), structure.Key);
    }

    public static T[] ReadArraySafe<T>(this IMemory memory, long address, uint size) where T : struct
    {
        uint arraySize = memory.Read<uint>(address + 0x1C);
        arraySize = Math.Min(size, arraySize);
        return memory.Read<T>(address + 0x20, arraySize);
    }
}
