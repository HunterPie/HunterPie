using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using System.Runtime.CompilerServices;
using System.Text;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Player;

public sealed class MHRSunbreakDemoPlayer : CommonPlayer
{
    #region Private
    private string _name;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    #endregion

    public override string Name
    {
        get => _name;
        protected set
        {
            if (value != _name)
            {
                _name = value;
                FindPlayerSaveSlot();
                this.Dispatch(value is ""
                    ? _onLogout
                    : _onLogin);

            }
        }
    }

    public override int HighRank
    {
        get => 0;
        protected set => throw new NotSupportedException();
    }

    public override int StageId
    {
        get => 0;
        protected set => throw new NotSupportedException();
    }

    public override bool InHuntingZone => false;

    public override IParty Party => throw new NotSupportedException();

    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override int MasterRank { get; protected set; }

    public override IStaminaComponent Stamina { get; } = new StaminaComponent();

    public override IHealthComponent Health { get; } = new HealthComponent();

    public override IWeapon Weapon { get; protected set; }

    public MHRSunbreakDemoPlayer(IProcessManager process) : base(process) { }

    [ScannableMethod]
    private void GetStageData()
    {
        long stageAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("STAGE_ADDRESS"),
            AddressMap.Get<int[]>("STAGE_OFFSETS")
        );

        int[] stageIds = Process.Memory.Read<int>(stageAddress + 0x150, 4);

        bool isVillage = stageIds[0] == 4;
        bool isMainMenu = stageIds[0] == 0;

        int villageId = stageIds[1];
        int huntId = stageIds[3];

        int zoneId = isMainMenu switch
        {
            true => -1,
            false => isVillage
            ? villageId
            : huntId + 200
        };

        StageId = zoneId;
    }

    [ScannableMethod]
    private void GetPlayerSaveData()
    {
        if (StageId is (-1) or 199)
        {
            Name = "";
            return;
        }

        long currentPlayerSaveAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            AddressMap.Get<int[]>("CHARACTER_OFFSETS")
        );

        long namePtr = Process.Memory.Read<long>(currentPlayerSaveAddress);
        int nameLength = Process.Memory.Read<int>(namePtr + 0x10);
        string name = Process.Memory.Read(namePtr + 0x14, (uint)(nameLength * 2), encoding: Encoding.Unicode);

        Name = name;
    }

    private void FindPlayerSaveSlot()
    {
        if (StageId is (-1) or 199)
        {
            Name = "";
            return;
        }

        long currentPlayerSaveAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            AddressMap.Get<int[]>("CHARACTER_OFFSETS")
        );
        long namePtr = Process.Memory.Read<long>(currentPlayerSaveAddress);

        long saveAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("SAVE_ADDRESS"),
            AddressMap.Get<int[]>("SAVE_OFFSETS")
        );

        for (int i = 0; i < 3; i++)
        {
            int[] nameOffsets = { (i * 8) + 0x20, 0x10 };

            long saveNamePtr = Process.Memory.Deref<long>(saveAddress, nameOffsets);

            if (saveNamePtr == namePtr)
                return;
        }
    }

    [ScannableMethod]
    private void AbnormalitiesCleanup()
    {
        long debuffsPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("DEBUFF_ABNORMALITIES_OFFSETS")
        );

        if (debuffsPtr == 0)
            ClearAbnormalities(_abnormalities);
    }

    [ScannableMethod]
    private void GetPlayerConsumableAbnormalities()
    {

        long consumableBuffs = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("CONS_ABNORMALITIES_OFFSETS")
        );

        if (consumableBuffs == 0)
            return;

        AbnormalitySchema[] consumableSchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Consumables);

        foreach (AbnormalitySchema schema in consumableSchemas)
        {
            int abnormSubId = schema.DependsOn switch
            {
                0 => 0,
                _ => Process.Memory.Read<int>(consumableBuffs + schema.DependsOn)
            };

            MHRAbnormalityData abnormality = new();

            if (abnormSubId == schema.WithValue)
            {
                MHRAbnormalityStructure abnormalityStructure = Process.Memory.Read<MHRAbnormalityStructure>(consumableBuffs + schema.Offset);
                abnormality = MHRAbnormalityAdapter.Convert(schema, abnormalityStructure);
            }

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRConsumableAbnormality, MHRAbnormalityData>(
                _abnormalities,
                schema,
                abnormality.Timer,
                abnormality
            );
        }
    }

    [ScannableMethod]
    private void GetPlayerDebuffAbnormalities()
    {

        long debuffsPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("DEBUFF_ABNORMALITIES_OFFSETS")
        );

        if (debuffsPtr == 0)
            return;

        AbnormalitySchema[] debuffSchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Debuffs);

        foreach (AbnormalitySchema schema in debuffSchemas)
        {
            int abnormSubId = schema.DependsOn switch
            {
                0 => 0,
                _ => Process.Memory.Read<int>(debuffsPtr + schema.DependsOn)
            };

            MHRAbnormalityData abnormality = new();

            // Only read memory if the required sub Id is the required one for this abnormality
            if (abnormSubId == schema.WithValue)
            {
                MHRAbnormalityStructure abnormalityStructure = Process.Memory.Read<MHRAbnormalityStructure>(debuffsPtr + schema.Offset);
                abnormality = MHRAbnormalityAdapter.Convert(schema, abnormalityStructure);
            }

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRDebuffAbnormality, MHRAbnormalityData>(
                _abnormalities,
                schema,
                abnormality.Timer,
                abnormality
            );
        }
    }

    [ScannableMethod]
    private void GetPlayerSongAbnormalities()
    {

        long songBuffsPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("HH_ABNORMALITIES_OFFSETS")
        );

        if (songBuffsPtr == 0)
            return;

        uint songBuffsLength = Process.Memory.Read<uint>(songBuffsPtr + 0x1C);
        long[] songBuffPtrs = Process.Memory.Read<long>(songBuffsPtr + 0x20, songBuffsLength);

        DerefSongBuffs(songBuffPtrs);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void DerefSongBuffs(long[] buffs)
    {
        int id = 0;
        AbnormalitySchema[] schemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Songs);
        foreach (long buffPtr in buffs)
        {
            MHRHHAbnormality abnormality = Process.Memory.Read<MHRHHAbnormality>(buffPtr);
            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            AbnormalitySchema maybeSchema = schemas[id];

            if (maybeSchema is AbnormalitySchema schema)
                HandleAbnormality<MHRSongAbnormality, MHRHHAbnormality>(
                    _abnormalities,
                    schema,
                    abnormality.Timer,
                    abnormality
                );

            id++;
        }
    }
}
