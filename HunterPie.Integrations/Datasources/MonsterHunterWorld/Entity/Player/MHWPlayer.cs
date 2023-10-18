using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.DTO;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data;
using HunterPie.Core.Game.Data.Schemas;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Definition;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWorld.Utils;
using HealthData = HunterPie.Integrations.Datasources.Common.Definition.HealthData;
using SpecializedTool = HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player.MHWSpecializedTool;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Entity.Player;

public sealed class MHWPlayer : CommonPlayer
{
    #region consts
    private static readonly Stage[] PeaceZones =
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
    private readonly MHWSkillService _skillService;

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

    public override string Name { get; protected set; } = string.Empty;

    public override int HighRank
    {
        get => _highRank;
        protected set
        {
            if (value != _highRank)
            {
                _highRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }

    public override int MasterRank
    {
        get => _masterRank;
        protected set
        {
            if (value != _masterRank)
            {
                _masterRank = value;
                this.Dispatch(_onLevelChange, new LevelChangeEventArgs(this));
            }
        }
    }

    public int PlayTime { get; private set; }

    public Stage ZoneId
    {
        get => _zoneId;
        set
        {
            if (value != _zoneId)
            {
                if (PeaceZones.Contains(value) && !PeaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageEnter);
                else if (!PeaceZones.Contains(value) && PeaceZones.Contains(_zoneId))
                    this.Dispatch(_onVillageLeave);

                _zoneId = value;
                this.Dispatch(_onStageUpdate);
            }
        }
    }

    public SpecializedTool[] Tools { get; } = { new(), new() };

    public bool IsLoggedOn => _playerSaveAddress != 0;

    public override int StageId
    {
        get => (int)ZoneId;
        protected set => throw new NotSupportedException();
    }

    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IParty Party => _party;

    public override bool InHuntingZone => ZoneId != Stage.MainMenu
                                 && !PeaceZones.Contains(_zoneId);

    public override IHealthComponent Health => _health;

    public override IStaminaComponent Stamina => _stamina;

    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                IWeapon temp = _weapon;
                _weapon = value;
                this.Dispatch(_onWeaponChange, new WeaponChangeEventArgs(temp, _weapon));

                if (temp is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }

    public MHWHarvestBox HarvestBox { get; } = new();

    public MHWSteamworks Steamworks { get; } = new();

    public MHWArgosy Argosy { get; } = new();

    public MHWTailraiders Tailraiders { get; } = new();
    #endregion

    internal MHWPlayer(IProcessManager process) : base(process)
    {
        _weapon = CreateDefaultWeapon(process);
        _skillService = new MHWSkillService(process);
    }

    private static IWeapon CreateDefaultWeapon(IProcessManager process)
    {
        var weapon = new MHWMeleeWeapon(process, WeaponType.Greatsword);
        ScanManager.Add(weapon);
        return weapon;
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
        const long nextPlayerSave = 0x26CC00;
        long currentPlayerSaveHeader =
            Process.Memory.Read<long>(firstSaveAddress) + (nextPlayerSave * currentSaveSlot);

        if (currentPlayerSaveHeader == _playerSaveAddress)
            return;

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
            MaxPossibleHealth = Math.Max(_skillService.ToMaximumHealthPossible(), hudStructure.MaxHealth),
        };

        _health.Update(healthData);

        var staminaData = new StaminaData
        {
            MaxStamina = hudStructure.MaxStamina,
            Stamina = hudStructure.Stamina,
            MaxPossibleStamina = Math.Max(_skillService.ToMaximumStaminaPossible(), hudStructure.MaxStamina)
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

        data.WeaponType = (WeaponType)Process.Memory.Read<byte>(address);

        if (data.WeaponType == _weaponId)
            return;

        if (Weapon is Scannable scannable)
            ScanManager.Remove(scannable);

        IWeapon? weaponInstance = data.WeaponType switch
        {
            WeaponType.ChargeBlade => new MHWChargeBlade(Process, _skillService),
            WeaponType.InsectGlaive => new MHWInsectGlaive(Process),
            WeaponType.Bow => new MHWBow(),
            WeaponType.HeavyBowgun => new MHWHeavyBowgun(),
            WeaponType.LightBowgun => new MHWLightBowgun(),
            WeaponType.DualBlades => new MHWDualBlades(Process),

            WeaponType.Greatsword
                or WeaponType.SwordAndShield
                or WeaponType.Longsword
                or WeaponType.Hammer
                or WeaponType.HuntingHorn
                or WeaponType.Lance
                or WeaponType.GunLance
                or WeaponType.SwitchAxe => new MHWMeleeWeapon(Process, data.WeaponType),

            _ => null
        };

        switch (weaponInstance)
        {
            case null:
                return;

            case Scannable weaponScannable:
                ScanManager.Add(weaponScannable);
                break;
        }

        Weapon = weaponInstance;

        _weaponId = data.WeaponType;
    }

    [ScannableMethod]
    private void GetParty()
    {
        var questInformation = (QuestState)Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_DATA_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATE_OFFSETS")
        );

        if (questInformation.IsQuestOver())
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
        {
            _party.ClearExcept(0);

            _party.Update(0, new MHWPartyMemberData
            {
                Name = Name,
                Weapon = _weaponId,
                Damage = 0,
                Slot = 0,
                IsMyself = true,
                MasterRank = MasterRank
            });

            return;
        }

        _party.Remove(0);

        MHWPartyMemberStructure[] partyMembers = Process.Memory.Read<MHWPartyMemberStructure>(partyInformationPtr, 4);

        long localPlayerReference = 0;
        int index = -1;
        foreach (MHWPartyMemberStructure partyMember in partyMembers)
        {
            index++;

            string name = Process.Memory.Read(partyMember.Address + 0x49, 32);

            bool isInParty = !string.IsNullOrEmpty(name);

            if (!isInParty)
            {
                _party.Remove(partyMember.Address);
                continue;
            }

            MHWPartyMemberLevelStructure levels = Process.Memory.Read<MHWPartyMemberLevelStructure>(partyMember.Address + 0x70);

            bool isLocalPlayer = name == Name && levels.HighRank == HighRank && levels.MasterRank == MasterRank;

            if (isLocalPlayer)
                localPlayerReference = partyMember.Address;

            var data = new MHWPartyMemberData
            {
                Name = name,
                Weapon = isLocalPlayer ? _weaponId : (WeaponType)Process.Memory.Read<byte>(partyMember.Address + 0x7C),
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
    private void GetHarvestBoxData()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        long harvestBoxPtr = PlayerSaveAddress + 0x103068;

        MHWFertilizerStructure[] fertilizers = Memory.Read<MHWFertilizerStructure>(harvestBoxPtr, 4);
        MHWItemStructure[] harvestBoxItems = Memory.Read<MHWItemStructure>(harvestBoxPtr + 0x50, 50);

        var data = new MHWHarvestBoxData(
            Items: harvestBoxItems,
            Fertilizers: fertilizers
        );

        HarvestBox.Update(data);
    }

    [ScannableMethod]
    private void GetSteamFuel()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        MHWSteamFuelStructure structure = Memory.Read<MHWSteamFuelStructure>(PlayerSaveAddress + 0x102FDC);

        var data = new MHWSteamFuelData(
            NaturalFuel: structure.NaturalFuel,
            StoredFuel: structure.StoredFuel
        );

        Steamworks.Update(data);
    }

    [ScannableMethod]
    private void GetArgosy()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        long argosyPtr = PlayerSaveAddress + 0x1034C0;

        byte argosyDays = Memory.Read<byte>(argosyPtr);
        bool isInTown = argosyDays < 250;

        if (!isInTown)
            argosyDays = (byte)(byte.MaxValue - argosyDays + 1);

        var data = new MHWArgosyData(
            DaysLeft: argosyDays,
            IsInTown: isInTown
        );

        Argosy.Update(data);
    }

    [ScannableMethod]
    private void GetTailraiders()
    {
        if (PlayerSaveAddress.IsNullPointer())
            return;

        long tailraidersPtr = PlayerSaveAddress + 0x1034DC;

        byte tailraidersQuests = Memory.Read<byte>(tailraidersPtr);
        bool isDeployed = tailraidersQuests != byte.MaxValue;
        int questsLeft = isDeployed ? Tailraiders.MaxDays - tailraidersQuests : 0;

        var data = new MHWTailraidersData(
            QuestsLeft: questsLeft,
            IsDeployed: isDeployed
        );

        Tailraiders.Update(data);
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
            ClearAbnormalities(_abnormalities);
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

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
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

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
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

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private void GetConsumableAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Consumables);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = new();

            int abnormSubId = abnormalitySchema.DependsOn switch
            {
                0 => 0,
                _ => Memory.Read<int>(baseAddress + abnormalitySchema.DependsOn)
            };

            if (abnormSubId == abnormalitySchema.WithValue)
            {
                structure = Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

                if (abnormalitySchema.IsInfinite)
                    structure.Timer = 1;
            }

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    private void GetSkillAbnormalities(long baseAddress)
    {
        AbnormalitySchema[] abnormalitySchemas = AbnormalityData.GetAllAbnormalitiesFromCategory(AbnormalityData.Skills);

        foreach (AbnormalitySchema abnormalitySchema in abnormalitySchemas)
        {
            MHWAbnormalityStructure structure = Process.Memory.Read<MHWAbnormalityStructure>(baseAddress + abnormalitySchema.Offset);

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
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

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
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

            HandleAbnormality<MHWAbnormality, MHWAbnormalityStructure>(
                _abnormalities,
                abnormalitySchema,
                structure.Timer,
                structure
            );
        }
    }

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        foreach (EntityDamageData entity in entities)
            // For now we are only tracking local player.
            if (entity.Entity.Index == 0)
                _party.Update(_localPlayerAddress, entity);
    }

    public override void Dispose()
    {
        _skillService.Dispose();
        Tools.DisposeAll();
        HarvestBox.Dispose();
        Steamworks.Dispose();
        base.Dispose();
    }
}