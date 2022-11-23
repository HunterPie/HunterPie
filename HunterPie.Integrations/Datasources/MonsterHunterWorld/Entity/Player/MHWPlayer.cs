using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Utils;
using HunterPie.Core.Logger;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Definition;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;
using System.Runtime.CompilerServices;
using HealthData = HunterPie.Integrations.Datasources.Common.Definition.HealthData;
using SpecializedTool = HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.MHWSpecializedTool;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public class MHWPlayer : Scannable, IPlayer, IEventDispatcher
{
    #region consts
    private static readonly Stage[] peaceZones =
    {
        Stage.Astera,
        Stage.AsteraGatheringHub,
        Stage.ResearchBase,
        Stage.Seliana,
        Stage.SelianaGatheringHub,
        Stage.LivingQuarters,
        Stage.PrivateQuarters,
        Stage.PrivateSuite,
        Stage.SelianaRoom
    };
    #endregion

    #region Private fields

    private long _playerSaveAddress;
    private long _localPlayerAddress;
    private Stage _zoneId;
    private Weapon _weaponId = WeaponType.None;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    private readonly MHWParty _party = new();
    private IWeapon _weapon;
    private int _highRank;
    private int _masterRank;
    private readonly HealthComponent _health = new();
    private readonly StaminaComponent _stamina = new();
    private MHWGearSkill[] _skills;
    #endregion

    #region Public Properties

    public long PlayerSaveAddress
    {
        get => _playerSaveAddress;
        private set
        {
            if (value != _playerSaveAddress)
            {
                _playerSaveAddress = value;

                this.Dispatch(
                    value != 0
                    ? _onLogin
                    : _onLogout
                );

            }
        }
    }
    public string Name { get; private set; }
    public int HighRank
    {
        get => _highRank;
        private set
        {
            if (value != _highRank)
            {
                _highRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }
    public int MasterRank
    {
        get => _masterRank;
        private set
        {
            if (value != _masterRank)
            {
                _masterRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }
    public int PlayTime { get; private set; }

    /// <summary>
    /// Player stage id
    /// </summary>
    public Stage ZoneId
    {
        get => _zoneId;
        set
        {
            if (value != _zoneId)
            {
                if (peaceZones.Contains(value) && !peaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageEnter);
                else if (!peaceZones.Contains(value) && peaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageLeave);

                _zoneId = value;
                this.Dispatch(_onStageUpdate);
            }
        }
    }

    public SpecializedTool[] Tools { get; } = { new(), new() };

    public bool IsLoggedOn => _playerSaveAddress != 0;

    public int StageId => (int)ZoneId;

    public IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public IParty Party => _party;

    public bool InHuntingZone => ZoneId != Stage.MainMenu
                                 && !peaceZones.Contains(_zoneId);

    public IHealthComponent Health => _health;

    public IStaminaComponent Stamina => _stamina;

    public IWeapon Weapon
    {
        get => _weapon;
        private set
        {
            if (value != _weapon)
            {
                IWeapon temp = _weapon;
                _weapon = value;
                this.Dispatch(_onWeaponChange, new WeaponChangeEventArgs(temp, _weapon));
            }
        }
    }

    #endregion

    #region Events

    private readonly SmartEvent<EventArgs> _onLogin = new();
    public event EventHandler<EventArgs> OnLogin
    {
        add => _onLogin.Hook(value);
        remove => _onLogin.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onLogout = new();
    public event EventHandler<EventArgs> OnLogout
    {
        add => _onLogout.Hook(value);
        remove => _onLogout.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onDeath = new();
    public event EventHandler<EventArgs> OnDeath
    {
        add => _onDeath.Hook(value);
        remove => _onDeath.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onActionUpdate = new();
    public event EventHandler<EventArgs> OnActionUpdate
    {
        add => _onActionUpdate.Hook(value);
        remove => _onActionUpdate.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onStageUpdate = new();
    public event EventHandler<EventArgs> OnStageUpdate
    {
        add => _onStageUpdate.Hook(value);
        remove => _onStageUpdate.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onVillageEnter = new();
    public event EventHandler<EventArgs> OnVillageEnter
    {
        add => _onVillageEnter.Hook(value);
        remove => _onVillageEnter.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onVillageLeave = new();
    public event EventHandler<EventArgs> OnVillageLeave
    {
        add => _onVillageLeave.Hook(value);
        remove => _onVillageLeave.Unhook(value);
    }

    private readonly SmartEvent<EventArgs> _onAilmentUpdate = new();
    public event EventHandler<EventArgs> OnAilmentUpdate
    {
        add => _onAilmentUpdate.Hook(value);
        remove => _onAilmentUpdate.Unhook(value);
    }

    private readonly SmartEvent<WeaponChangeEventArgs> _onWeaponChange = new();
    public event EventHandler<WeaponChangeEventArgs> OnWeaponChange
    {
        add => _onWeaponChange.Hook(value);
        remove => _onWeaponChange.Unhook(value);
    }

    private readonly SmartEvent<IAbnormality> _onAbnormalityStart = new();
    public event EventHandler<IAbnormality> OnAbnormalityStart
    {
        add => _onAbnormalityStart.Hook(value);
        remove => _onAbnormalityStart.Unhook(value);
    }

    private readonly SmartEvent<IAbnormality> _onAbnormalityEnd = new();
    public event EventHandler<IAbnormality> OnAbnormalityEnd
    {
        add => _onAbnormalityEnd.Hook(value);
        remove => _onAbnormalityEnd.Unhook(value);
    }

    private readonly SmartEvent<LevelChangeEventArgs> _onLevelChange = new();
    public event EventHandler<LevelChangeEventArgs> OnLevelChange
    {
        add => _onLevelChange.Hook(value);
        remove => _onLevelChange.Unhook(value);
    }

    #endregion

    internal MHWPlayer(IProcessManager process) : base(process)
    {
        _weapon = new MHWMeleeWeapon(process, WeaponType.Greatsword);
    }

    [ScannableMethod(typeof(ZoneData))]
    private void GetZoneData()
    {
        ZoneData data = new();

        long zoneAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("ZONE_OFFSET"),
            AddressMap.Get<int[]>("ZoneOffsets")
        );

        data.ZoneId = (Stage)Process.Memory.Read<int>(zoneAddress);

        Next(ref data);

        ZoneId = data.ZoneId;
    }

    [ScannableMethod(typeof(PlayerInformationData))]
    private void GetBasicData()
    {
        PlayerInformationData data = new();
        if (ZoneId == Stage.MainMenu)
        {
            PlayerSaveAddress = 0;
            return;
        }

        long firstSaveAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("LEVEL_OFFSET"),
            AddressMap.Get<int[]>("LevelOffsets")
        );

        uint currentSaveSlot = Process.Memory.Read<uint>(firstSaveAddress + 0x44);
        long nextPlayerSave = 0x27E9F0;
        long currentPlayerSaveHeader =
            Process.Memory.Read<long>(firstSaveAddress) + (nextPlayerSave * currentSaveSlot);

        if (currentPlayerSaveHeader != _playerSaveAddress)
        {
            data.Name = Process.Memory.Read(currentPlayerSaveHeader + 0x50, 32);
            data.HighRank = Process.Memory.Read<short>(currentPlayerSaveHeader + 0x90);
            data.MasterRank = Process.Memory.Read<short>(currentPlayerSaveHeader + 0xD4);
            data.PlayTime = Process.Memory.Read<int>(currentPlayerSaveHeader + 0xA0);

            Next(ref data);

            Name = data.Name;
            HighRank = data.HighRank;
            MasterRank = data.MasterRank;
            PlayTime = data.PlayTime;

            PlayerSaveAddress = currentPlayerSaveHeader;
        }
    }

    [ScannableMethod]
    private void GetGearSkills()
    {
        long gearSkillsPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("GEAR_SKILL_OFFSETS")
        );

        _skills = Process.Memory.Read<MHWGearSkill>(gearSkillsPtr, 226);
    }

    [ScannableMethod]
    private void GetPlayerHudData()
    {
        long basicPlayerDataPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_BASIC_INFORMATION_OFFSETS")
        );

        MHWHudStructure hudStructure = Process.Memory.Read<MHWHudStructure>(basicPlayerDataPtr);

        MHWHealingStructure totalHealings = Process.Memory.Read<MHWHealingStructure>(hudStructure.HealingArrayPointer + 0xEBB0, 4)
                                                           .ToTotal();

        var healthData = new HealthData
        {
            MaxHealth = hudStructure.MaxHealth,
            Health = hudStructure.Health,
            RecoverableHealth = hudStructure.RecoverableHealth,
            Heal = hudStructure.Health + totalHealings.MaxHeal - totalHealings.Heal,
            MaxPossibleHealth = _skills.ToMaximumHealthPossible(),
        };

        _health.Update(healthData);

        var staminaData = new StaminaData
        {
            MaxStamina = hudStructure.MaxStamina,
            Stamina = hudStructure.Stamina,
            MaxPossibleStamina = _skills.ToMaximumStaminaPossible()
        };

        _stamina.Update(staminaData);
    }

    [ScannableMethod]
    private void GetWeaponData()
    {
        PlayerEquipmentData data = new();

        if (!IsLoggedOn)
            return;

        long address = Process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );

        data.WeaponType = (Weapon)Process.Memory.Read<byte>(address);

        if (data.WeaponType == _weaponId)
            return;

        if (Weapon is Scannable scannable)
            ScanManager.Remove(scannable);

        IWeapon? weaponInstance = null;
        if (data.WeaponType.IsMelee())
        {
            var meleeWeapon = new MHWMeleeWeapon(Process, data.WeaponType);
            weaponInstance = meleeWeapon;

            ScanManager.Add(meleeWeapon);
        }
        else
            weaponInstance = (data.WeaponType) switch
            {
                WeaponType.Bow => new MHWBow(),
                WeaponType.HeavyBowgun => new MHWHeavyBowgun(),
                WeaponType.LightBowgun => new MHWLightBowgun(),
                _ => null
            };

        if (weaponInstance is not null)
            Weapon = weaponInstance;

        _weaponId = data.WeaponType;
    }

    [ScannableMethod]
    private void GetParty()
    {
        int questInformation = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATE_OFFSETS")
        );

        if (questInformation.IsMHWQuestOver())
            return;

        long partyInformationPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("PARTY_ADDRESS"),
            AddressMap.Get<int[]>("PARTY_OFFSETS")
        );

        long damageInformation = Process.Memory.Read(
            AddressMap.GetAbsolute("DAMAGE_ADDRESS"),
            AddressMap.Get<int[]>("DAMAGE_OFFSETS")
        );

        int partySize = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("SESSION_OFFSET"),
            AddressMap.Get<int[]>("SESSION_PARTY_OFFSETS")
        );

        if (partySize is 0)
            _party.Update(0, new MHWPartyMemberData
            {
                Name = Name,
                Weapon = _weaponId,
                Damage = 0,
                Slot = 0,
                IsMyself = true,
                MasterRank = MasterRank
            });
        else
            _party.Remove(0);

        MHWPartyMemberStructure[] partyMembers = Process.Memory.Read<MHWPartyMemberStructure>(partyInformationPtr, 4);

        long localPlayerReference = 0;
        int index = -1;
        foreach (MHWPartyMemberStructure partyMember in partyMembers)
        {
            index++;

            if (index >= partySize)
            {
                _party.Remove(partyMember.Address);
                continue;
            }

            string name = Process.Memory.Read(partyMember.Address + 0x49, 32);

            if (string.IsNullOrEmpty(name))
                continue;

            bool isLocalPlayer = name == Name;

            if (isLocalPlayer)
                localPlayerReference = partyMember.Address;

            MHWPartyMemberLevelStructure levels = Process.Memory.Read<MHWPartyMemberLevelStructure>(partyMember.Address + 0x70);
            var data = new MHWPartyMemberData
            {
                Name = name,
                Weapon = isLocalPlayer ? _weaponId : (Weapon)Process.Memory.Read<byte>(partyMember.Address + 0x7C),
                Damage = Process.Memory.Read<int>(damageInformation + (index * 0x2A0)),
                Slot = index,
                IsMyself = isLocalPlayer,
                MasterRank = levels.MasterRank
            };

            _party.Update(partyMember.Address, data);
        }

        _localPlayerAddress = localPlayerReference;
    }

    [ScannableMethod]
    private void GetMantlesData()
    {
        long address = Process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );
        SpecializedToolType[] ids = Process.Memory.Read<int>(address + 0x34, 2)
            .Select(e => (SpecializedToolType)e)
            .ToArray();

        long equipmentAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            AddressMap.Get<int[]>("EQUIPMENT_OFFSETS")
        );

        const int specializedTools = 20;
        float[] cooldowns = Process.Memory.Read<float>(equipmentAddress + 0x99C, specializedTools * 2);
        float[] timers = Process.Memory.Read<float>(equipmentAddress + 0xA8C, specializedTools * 2);

        for (int i = 0; i < Tools.Length; i++)
        {
            int id = (int)ids[i];
            var tool = new MHWSpecializedToolStructure
            {
                Id = ids[i],
                Timer = timers.ElementAtOrDefault(id),
                MaxTimer = timers.ElementAtOrDefault(id + specializedTools),
                Cooldown = cooldowns.ElementAtOrDefault(id),
                MaxCooldown = cooldowns.ElementAtOrDefault(id + specializedTools),
            };
            IUpdatable<MHWSpecializedToolStructure> mhwTool = Tools[i];
            mhwTool.Update(tool);
        }
    }

    [ScannableMethod]
    private void GetAbnormalitiesCleanup()
    {
        long abnormalityBaseAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_OFFSETS")
        );

        if (!InHuntingZone || abnormalityBaseAddress == 0)
            ClearAbnormalities();
    }

    [ScannableMethod]
    private void GetAbnormalities()
    {
        if (!InHuntingZone)
            return;

        long abnormalityBaseAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_OFFSETS")
        );

        MHWAbnormalityStructure[] abnormalities = Process.Memory.Read<MHWAbnormalityStructure>(abnormalityBaseAddress + 0x38, 75);

        GetHuntingHornAbnormalities(abnormalities);
        GetOrchestraAbnormalities(abnormalities);
        GetDebuffAbnormalities(abnormalityBaseAddress);
        GetConsumableAbnormalities(abnormalityBaseAddress);
        GetSkillAbnormalities(abnormalityBaseAddress);
        GetFoodAbnormalities();
        GetGearAbnormalities();
    }

    private void GetHuntingHornAbnormalities(MHWAbnormalityStructure[] buffs)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Songs);
        int offsetFirstAbnormality = abnormalitySchemas[0].Offset;

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            // We can calculate the index of the abnormality based on their offset and the size of a float
            int index = (abnormalitySchema.Offset - offsetFirstAbnormality) / sizeof(float);
            MHWAbnormalityStructure structure = buffs[index];

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetOrchestraAbnormalities(MHWAbnormalityStructure[] buffs)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Orchestra);
        int offsetFirstAbnormality = abnormalitySchemas[0].Offset;
        int indexFirstOrchestraAbnormality = (offsetFirstAbnormality - 0x38) / sizeof(float);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            int index = ((abnormalitySchema.Offset - offsetFirstAbnormality) / sizeof(float)) + indexFirstOrchestraAbnormality;
            MHWAbnormalityStructure structure = buffs[index];

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetDebuffAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Debuffs);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = new();

            int abnormSubId = abnormalitySchema.DependsOn switch
            {
                0 => 0,
                _ => Process.Memory.Read<int>(baseAddress + abnormalitySchema.DependsOn)
            };

            if (abnormSubId == abnormalitySchema.WithValue)
                structure = Process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetConsumableAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Consumables);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = Process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetSkillAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Skills);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = Process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetFoodAbnormalities()
    {
        long canteenAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("CANTEEN_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_CANTEEN_OFFSETS")
        );

        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Foods);
        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = Process.Memory.Read<MHWAbnormalityStructure>(canteenAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetGearAbnormalities()
    {
        long gearAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_GEAR_OFFSETS")
        );

        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Gears);
        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = Process.Memory.Read<MHWAbnormalityStructure>(gearAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
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
            this.Dispatch(_onAbnormalityEnd, (IAbnormality)abnorm);
        }
        else if (_abnormalities.ContainsKey(schema.Id) && timer > 0)
        {

            var abnorm = (IUpdatable<S>)_abnormalities[schema.Id];
            abnorm.Update(newData);
        }
        else if (!_abnormalities.ContainsKey(schema.Id) && timer > 0)
        {
            if (schema.Icon == "ICON_MISSING")
                Log.Info($"Missing abnormality: {schema.Id}");

            var abnorm = (IUpdatable<S>)Activator.CreateInstance(typeof(T), schema);

            _abnormalities.Add(schema.Id, (IAbnormality)abnorm);
            abnorm.Update(newData);
            this.Dispatch(_onAbnormalityStart, (IAbnormality)abnorm);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ClearAbnormalities()
    {
        foreach (IAbnormality abnormality in _abnormalities.Values)
            this.Dispatch(_onAbnormalityEnd, abnormality);

        _abnormalities.Clear();
    }

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        foreach (EntityDamageData entity in entities)
            // For now we are only tracking local player.
            if (entity.Entity.Index == 0)
                _party.Update(_localPlayerAddress, entity);
    }
}