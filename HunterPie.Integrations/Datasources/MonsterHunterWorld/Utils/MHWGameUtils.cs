using HunterPie.Core.Game.Entity.Player.Skills;
using HunterPie.Core.Game.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;

public static class MHWGameUtils
{
    public static uint[] MaxQuestTimers = { 54000, 72000, 108000, 126000, 180000 };
    public const float DefaultMaxHealth = 150.0f;
    public const float DefaultMaxStamina = 150.0f;
    public const float StaminaIncrement = 50.0f;
    public static float[] HealthIncrements = { 0.0f, 15f, 30f, 50f };
    public const int HandicraftMultiplier = 10;
    public const int MaxHandicraft = 50;

    public static float ToSeconds(this uint self) => self / 60.0f;

    public static float ToSeconds(this ulong self) => self / 60.0f;

    public static MHWHealingStructure ToTotal(this MHWHealingStructure[] self)
    {
        var totalHealing = new MHWHealingStructure
        {
            Ref1 = self.First().Ref1,
            Ref2 = self.First().Ref2,
            Heal = 0,
            OldMaxHeal = 0,
            MaxHeal = 0,
            Stage = 2
        };

        foreach (MHWHealingStructure heal in self)
        {
            if (heal.Stage == 0)
                continue;

            float max = heal.Stage == 1 ? heal.MaxHeal * 2.5f : heal.MaxHeal;
            totalHealing.Heal += heal.Heal;
            totalHealing.MaxHeal += max;
        }

        return totalHealing;
    }

    public static float ToMaximumHealthPossible(this ISkillService skillService)
    {
        Skill vitalitySkill = skillService.GetSkillBy(0x15);
        return DefaultMaxHealth + HealthIncrements[Math.Min(vitalitySkill.Level, HealthIncrements.Length - 1)];
    }

    public static float ToMaximumStaminaPossible(this ISkillService skillService)
    {
        bool lunaFavor = skillService.GetSkillBy(0xA3).Level >= 2;
        bool anjaDominance = skillService.GetSkillBy(0xB6).Level >= 2;
        bool anjaWill = skillService.GetSkillBy(0x84).Level >= 4;

        bool staminaCapUp = lunaFavor || anjaDominance || anjaWill;

        return staminaCapUp ? DefaultMaxStamina + StaminaIncrement : DefaultMaxStamina;
    }

    public static int MaximumSharpness(this int[] sharpnesses, MHWGearSkill handicraft)
    {
        int handicraftLevel = Math.Min((int)handicraft.LevelGear, 5);
        return sharpnesses.Last(s => s > 0) - MaxHandicraft - (HandicraftMultiplier * handicraftLevel);
    }

    public static Sharpness ToSharpnessLevel(this int[] sharpnesses, int sharpness)
    {
        Sharpness level = Sharpness.Red;
        int previousThreshold = 0;
        foreach (int threshold in sharpnesses)
        {
            if (threshold == 0)
                return level;

            if (sharpness > previousThreshold && sharpness <= threshold)
                return level;

            level++;
            previousThreshold = threshold;
        }

        return level;
    }

    public static bool IsQuestOver(this QuestState state) => state switch
    {
        QuestState.Success or
            QuestState.Completed or
            QuestState.Failed or
            QuestState.Abandon or
            QuestState.Quit => true,
        _ => false
    };

    public static QuestStatus ToStatus(this QuestState state) => state switch
    {
        QuestState.InQuest => QuestStatus.InProgress,
        QuestState.Success or
        QuestState.Completed => QuestStatus.Success,
        QuestState.Failed => QuestStatus.Fail,
        QuestState.Abandon or
        QuestState.Quit => QuestStatus.Quit,
        _ => QuestStatus.None
    };

    public static KinsectBuff ToBuff(this KinsectBuffType type) => type switch
    {
        KinsectBuffType.Attack => KinsectBuff.Attack,
        KinsectBuffType.Speed => KinsectBuff.Speed,
        KinsectBuffType.Defense => KinsectBuff.Defense,
        KinsectBuffType.Heal => KinsectBuff.Heal,
        _ => KinsectBuff.None
    };

    public static PhialChargeLevel ToPhialChargeLevel(this float buildup)
    {
        return buildup switch
        {
            >= 72.0f => PhialChargeLevel.Overcharged,
            >= 46.0f => PhialChargeLevel.Red,
            >= 30.0f => PhialChargeLevel.Yellow,
            _ => PhialChargeLevel.None,
        };
    }
}
