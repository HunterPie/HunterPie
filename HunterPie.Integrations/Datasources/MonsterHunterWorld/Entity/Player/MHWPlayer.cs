using HunterPie.Core.Address.Map;
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
                    ? OnLogin
                    : OnLogout,
                    EventArgs.Empty
                );

                if (value != 0)
                    Log.Debug($"Logged in! Name: {Name}, HR: {HighRank}, MR: {MasterRank}, PlayTime: {PlayTime} seconds");
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
                this.Dispatch(OnLevelChange, new LevelChangeEventArgs(this));
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
                this.Dispatch(OnLevelChange, new LevelChangeEventArgs(this));
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
                    this.Dispatch(OnVillageEnter);
                else if (!peaceZones.Contains(value) && peaceZones.Contains(_zoneId))
                    this.Dispatch(OnVillageLeave);

                _zoneId = value;
                this.Dispatch(OnStageUpdate);
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
                this.Dispatch(OnWeaponChange, new WeaponChangeEventArgs(temp, _weapon));
            }
        }
    }

    #endregion

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
    public event EventHandler<LevelChangeEventArgs> OnLevelChange;

    internal MHWPlayer(IProcessManager process) : base(process)
    {
        _weapon = new MHWMeleeWeapon(process, WeaponType.Greatsword);
    }

    [ScannableMethod(typeof(ZoneData))]
    private void GetZoneData()
    {
        ZoneData data = new();

        long zoneAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("ZONE_OFFSET"),
            AddressMap.Get<int[]>("ZoneOffsets")
        );

        data.ZoneId = (Stage)_process.Memory.Read<int>(zoneAddress);

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

        long firstSaveAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("LEVEL_OFFSET"),
            AddressMap.Get<int[]>("LevelOffsets")
        );

        uint currentSaveSlot = _process.Memory.Read<uint>(firstSaveAddress + 0x44);
        long nextPlayerSave = 0x27E9F0;
        long currentPlayerSaveHeader =
            _process.Memory.Read<long>(firstSaveAddress) + (nextPlayerSave * currentSaveSlot);

        if (currentPlayerSaveHeader != _playerSaveAddress)
        {
            data.Name = _process.Memory.Read(currentPlayerSaveHeader + 0x50, 32);
            data.HighRank = _process.Memory.Read<short>(currentPlayerSaveHeader + 0x90);
            data.MasterRank = _process.Memory.Read<short>(currentPlayerSaveHeader + 0xD4);
            data.PlayTime = _process.Memory.Read<int>(currentPlayerSaveHeader + 0xA0);

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
        long gearSkillsPtr = _process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("GEAR_SKILL_OFFSETS")
        );

        _skills = _process.Memory.Read<MHWGearSkill>(gearSkillsPtr, 226);
    }

    [ScannableMethod]
    private void GetPlayerHudData()
    {
        long basicPlayerDataPtr = _process.Memory.Read(
            AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_BASIC_INFORMATION_OFFSETS")
        );

        MHWHudStructure hudStructure = _process.Memory.Read<MHWHudStructure>(basicPlayerDataPtr);

        MHWHealingStructure totalHealings = _process.Memory.Read<MHWHealingStructure>(hudStructure.HealingArrayPointer + 0xEBB0, 4)
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

        long address = _process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );

        data.WeaponType = (Weapon)_process.Memory.Read<byte>(address);

        if (data.WeaponType == _weaponId)
            return;

        if (Weapon is Scannable scannable)
            ScanManager.Remove(scannable);

        IWeapon? weaponInstance = null;
        if (data.WeaponType.IsMelee())
        {
            var meleeWeapon = new MHWMeleeWeapon(_process, data.WeaponType);
            weaponInstance = meleeWeapon;

            ScanManager.Add(meleeWeapon);
        }
        else
        {
            weaponInstance = (data.WeaponType) switch
            {
                WeaponType.Bow => new MHWBow(),
                WeaponType.HeavyBowgun => new MHWHeavyBowgun(),
                WeaponType.LightBowgun => new MHWLightBowgun(),
                _ => null
            };
        }

        if (weaponInstance is not null)
            Weapon = weaponInstance;

        _weaponId = data.WeaponType;
    }

    [ScannableMethod]
    private void GetParty()
    {
        int questInformation = _process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATE_OFFSETS")
        );

        if (questInformation.IsMHWQuestOver())
            return;

        long partyInformationPtr = _process.Memory.Read(
            AddressMap.GetAbsolute("PARTY_ADDRESS"),
            AddressMap.Get<int[]>("PARTY_OFFSETS")
        );

        long damageInformation = _process.Memory.Read(
            AddressMap.GetAbsolute("DAMAGE_ADDRESS"),
            AddressMap.Get<int[]>("DAMAGE_OFFSETS")
        );

        int partySize = _process.Memory.Deref<int>(
            AddressMap.GetAbsolute("SESSION_OFFSET"),
            AddressMap.Get<int[]>("SESSION_PARTY_OFFSETS")
        );

        if (partySize is 0)
        {
            _party.Update(0, new MHWPartyMemberData
            {
                Name = Name,
                Weapon = _weaponId,
                Damage = 0,
                Slot = 0,
                IsMyself = true,
                MasterRank = MasterRank
            });
        }
        else
            _party.Remove(0);

        MHWPartyMemberStructure[] partyMembers = _process.Memory.Read<MHWPartyMemberStructure>(partyInformationPtr, 4);

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

            string name = _process.Memory.Read(partyMember.Address + 0x49, 32);

            if (string.IsNullOrEmpty(name))
                continue;

            bool isLocalPlayer = name == Name;

            if (isLocalPlayer)
                localPlayerReference = partyMember.Address;

            MHWPartyMemberLevelStructure levels = _process.Memory.Read<MHWPartyMemberLevelStructure>(partyMember.Address + 0x70);
            var data = new MHWPartyMemberData
            {
                Name = name,
                Weapon = isLocalPlayer ? _weaponId : (Weapon)_process.Memory.Read<byte>(partyMember.Address + 0x7C),
                Damage = _process.Memory.Read<int>(damageInformation + (index * 0x2A0)),
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
        long address = _process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );
        SpecializedToolType[] ids = _process.Memory.Read<int>(address + 0x34, 2)
            .Select(e => (SpecializedToolType)e)
            .ToArray();

        long equipmentAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("EQUIPMENT_ADDRESS"),
            AddressMap.Get<int[]>("EQUIPMENT_OFFSETS")
        );

        const int specializedTools = 20;
        float[] cooldowns = _process.Memory.Read<float>(equipmentAddress + 0x99C, specializedTools * 2);
        float[] timers = _process.Memory.Read<float>(equipmentAddress + 0xA8C, specializedTools * 2);

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
        long abnormalityBaseAddress = _process.Memory.Read(
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

        long abnormalityBaseAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_OFFSETS")
        );

        MHWAbnormalityStructure[] abnormalities = _process.Memory.Read<MHWAbnormalityStructure>(abnormalityBaseAddress + 0x38, 75);

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
                _ => _process.Memory.Read<int>(baseAddress + abnormalitySchema.DependsOn)
            };

            if (abnormSubId == abnormalitySchema.WithValue)
                structure = _process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetConsumableAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Consumables);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = _process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetSkillAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Skills);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = _process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetFoodAbnormalities()
    {
        long canteenAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("CANTEEN_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_CANTEEN_OFFSETS")
        );

        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Foods);
        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = _process.Memory.Read<MHWAbnormalityStructure>(canteenAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(abnormalitySchema, structure.Timer, structure);
        }
    }

    private void GetGearAbnormalities()
    {
        long gearAddress = _process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITY_ADDRESS"),
            AddressMap.Get<int[]>("ABNORMALITY_GEAR_OFFSETS")
        );

        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Gears);
        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = _process.Memory.Read<MHWAbnormalityStructure>(gearAddress + abnormalitySchema.Offset);

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
                Log.Info($"Missing abnormality: {schema.Id}");

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

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        foreach (EntityDamageData entity in entities)
        {
            // For now we are only tracking local player.
            if (entity.Entity.Index == 0)
                _party.Update(_localPlayerAddress, entity);
        }
    }
}