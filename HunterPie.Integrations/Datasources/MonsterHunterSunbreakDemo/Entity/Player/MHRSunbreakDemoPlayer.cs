using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Events;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;
using System.Runtime.CompilerServices;
using System.Text;

namespace HunterPie.Integrations.Datasources.MonsterHunterSunbreakDemo.Entity.Player;

public class MHRSunbreakDemoPlayer : Scannable, IPlayer, IEventDispatcher
{
    #region Private
    private string _name;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    #endregion

    public string Name
    {
        get => _name;
        private set
        {
            if (value != _name)
            {
                _name = value;
                FindPlayerSaveSlot();
                this.Dispatch(value is ""
                    ? OnLogout
                    : OnLogin);

            }
        }
    }

    public int HighRank => 0;

    public int StageId { get; private set; }

    public bool InHuntingZone { get; private set; }

    public IParty Party { get; private set; }

    public IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public int MasterRank => 0;

    public IStaminaComponent Stamina { get; } = new StaminaComponent();

    public IHealthComponent Health { get; } = new HealthComponent();

    public double Heal => 0;

    public IWeapon Weapon { get; private set; }

    public event EventHandler<EventArgs>? OnLogin;
    public event EventHandler<EventArgs>? OnLogout;
    public event EventHandler<EventArgs>? OnDeath;
    public event EventHandler<EventArgs>? OnActionUpdate;
    public event EventHandler<EventArgs>? OnStageUpdate;
    public event EventHandler<EventArgs>? OnVillageEnter;
    public event EventHandler<EventArgs>? OnVillageLeave;
    public event EventHandler<EventArgs>? OnAilmentUpdate;
    public event EventHandler<WeaponChangeEventArgs>? OnWeaponChange;
    public event EventHandler<IAbnormality>? OnAbnormalityStart;
    public event EventHandler<IAbnormality>? OnAbnormalityEnd;
    public event EventHandler<HealthChangeEventArgs>? OnHealthChange;
    public event EventHandler<StaminaChangeEventArgs>? OnStaminaChange;
    public event EventHandler<LevelChangeEventArgs>? OnLevelChange;
    public event EventHandler<HealthChangeEventArgs>? OnHeal;

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
            ClearAbnormalities();
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

            MHRConsumableStructure abnormality = new();

            if (abnormSubId == schema.WithValue)
                abnormality = Process.Memory.Read<MHRConsumableStructure>(consumableBuffs + schema.Offset);

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRConsumableAbnormality, MHRConsumableStructure>(schema, abnormality.Timer, abnormality);
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

            MHRDebuffStructure abnormality = new();

            // Only read memory if the required sub Id is the required one for this abnormality
            if (abnormSubId == schema.WithValue)
                abnormality = Process.Memory.Read<MHRDebuffStructure>(debuffsPtr + schema.Offset);

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRDebuffAbnormality, MHRDebuffStructure>(schema, abnormality.Timer, abnormality);
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
                HandleAbnormality<MHRSongAbnormality, MHRHHAbnormality>(schema, abnormality.Timer, abnormality);

            id++;
        }
    }

    private void HandleAbnormality<T, S>(AbnormalitySchema schema, float timer, S newData)
        where T : IAbnormality, IUpdatable<S>
        where S : struct
    {
        if (_abnormalities.ContainsKey(schema.Id) && timer <= 0)
        {
            var abnorm = (IUpdatable<S>)_abnormalities[schema.Id];

            abnorm.Update(newData);

            _ = _abnormalities.Remove(schema.Id);
            this.Dispatch(OnAbnormalityEnd, (IAbnormality)abnorm);
        }
        else if (_abnormalities.ContainsKey(schema.Id) && timer > 0)
        {

            var abnorm = (IUpdatable<S>)_abnormalities[schema.Id];
            abnorm.Update(newData);
        }
        else if (!_abnormalities.ContainsKey(schema.Id) && timer > 0)
        {
            if (schema.Icon == "ICON_MISSING")
                Core.Logger.Log.Info($"Missing abnormality: {schema.Id}");

            var abnorm = (IUpdatable<S>)Activator.CreateInstance(typeof(T), schema);

            _abnormalities.Add(schema.Id, (IAbnormality)abnorm);
            abnorm.Update(newData);
            this.Dispatch(OnAbnormalityStart, (IAbnormality)abnorm);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearAbnormalities()
    {
        foreach (IAbnormality abnormality in _abnormalities.Values)
            this.Dispatch(OnAbnormalityEnd, abnormality);

        _abnormalities.Clear();
    }
}
