using HunterPie.Core.Address.Map;
using HunterPie.Core.Architecture.Events;
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
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Utils;
using HunterPie.Core.Native.IPC.Models.Common;
using HunterPie.Integrations.Datasources.Common.Definition;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.Common.Entity.Player.Vitals;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Definitions;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Enums;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Environment.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using System.Runtime.CompilerServices;
using System.Text;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Entity.Player;

public sealed class MHRPlayer : CommonPlayer
{
    #region Private
    private int _saveSlotId;
    private string _name;
    private int _stageId = -1;
    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    private readonly MHRParty _party = new();
    private MHRStageStructure _stageData;
    private MHRStageStructure _lastStageData;
    private readonly HealthComponent _health = new();
    private readonly StaminaComponent _stamina = new();
    private int _highRank;
    private int _masterRank;
    private IWeapon _weapon;
    private Weapon _weaponId = WeaponType.None;
    private readonly Dictionary<int, MHREquipmentSkillStructure> _armorSkills = new(46);
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

                this.Dispatch(
                    value != ""
                        ? _onLogin
                        : _onLogout
                );
            }
        }
    }

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

    public override int StageId
    {
        get => _stageId;
        protected set
        {
            if (value != _stageId)
            {
                if (_stageData.IsVillage() && value != 5 && (_lastStageData.IsHuntingZone() || StageId == 5 || _lastStageData.IsIrrelevantStage()))
                    this.Dispatch(_onVillageEnter);
                else if (_stageData.IsHuntingZone() || value == 5)
                    this.Dispatch(_onVillageLeave);

                _stageId = value;
                this.Dispatch(_onStageUpdate);
            }
        }
    }

    public override bool InHuntingZone => _stageData.IsHuntingZone() || StageId == 5;

    public override IParty Party => _party;

    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IHealthComponent Health => _health;

    public override IStaminaComponent Stamina => _stamina;

    public MHRWirebug[] Wirebugs { get; } = { new(), new(), new() };

    public MHRArgosy Argosy { get; } = new();

    public MHRTrainingDojo TrainingDojo { get; } = new();

    public MHRMeowmasters Meowmasters { get; } = new();

    public MHRCohoot Cohoot { get; } = new();

    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (value != _weapon)
            {
                IWeapon lastWeapon = _weapon;
                _weapon = value;
                this.Dispatch(_onWeaponChange, new WeaponChangeEventArgs(lastWeapon, _weapon));

                if (lastWeapon is IDisposable disposable)
                    disposable.Dispose();
            }
        }
    }

    public Scroll SwitchScroll { get; private set; }

    #region Events

    private readonly SmartEvent<MHRWirebug[]> _onWirebugsRefresh = new();
    public event EventHandler<MHRWirebug[]> OnWirebugsRefresh
    {
        add => _onWirebugsRefresh.Hook(value);
        remove => _onWirebugsRefresh.Unhook(value);
    }

    #endregion

    public MHRPlayer(IProcessManager process) : base(process)
    {
        _weapon = new MHRMeleeWeapon(process, WeaponType.Greatsword);
    }

    // TODO: Add DTOs for middlewares

    [ScannableMethod]
    private void GetStageData()
    {
        long stageAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("STAGE_ADDRESS"),
            AddressMap.Get<int[]>("STAGE_OFFSETS")
        );

        if (stageAddress == 0x00000000)
            return;

        MHRStageStructure stageData = Process.Memory.Read<MHRStageStructure>(stageAddress + 0x60);

        int zoneId = stageData.IsMainMenu()
            ? -1
            : stageData.IsVillage()
            ? stageData.VillageId
            : stageData.IsLoadingScreen() ? -2 : stageData.IsSelectingCharacter() ? 199 : stageData.HuntingId + 200;
        MHRStageStructure tempStageData = _stageData;
        _stageData = stageData;
        _lastStageData = tempStageData;

        StageId = zoneId;
    }

    [ScannableMethod]
    private void GetPlayerSaveData()
    {
        if (_stageData.IsMainMenu())
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
        if (_stageData.IsMainMenu())
        {
            Name = "";
            _saveSlotId = -1;
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
            {
                _saveSlotId = i;
                return;
            }
        }
    }

    [ScannableMethod]
    private void GetPlayerLevel()
    {
        if (_saveSlotId < 0)
            return;

        long saveAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("SAVE_ADDRESS"),
            AddressMap.Get<int[]>("SAVE_OFFSETS")
        );

        int[] levelOffsets = { (_saveSlotId * 8) + 0x20, 0x18 };

        MHRPlayerLevelStructure level = Process.Memory.Deref<MHRPlayerLevelStructure>(saveAddress, levelOffsets);

        HighRank = level.HighRank;
        MasterRank = level.MasterRank;
    }

    [ScannableMethod]
    private void GetPlayerWeaponData()
    {
        long weaponIdPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("WEAPON_ADDRESS"),
            AddressMap.Get<int[]>("WEAPON_OFFSETS")
        );

        int weaponId = Process.Memory.Read<int>(weaponIdPtr + 0x8C);

        WeaponType weapon = weaponId.ToWeaponId();

        if (weapon == _weaponId)
            return;

        if (Weapon is Scannable scannable)
            ScanManager.Remove(scannable);

        IWeapon? weaponInstance;
        if (weapon.IsMelee())
        {
            var meleeWeapon = new MHRMeleeWeapon(Process, weapon);
            weaponInstance = meleeWeapon;

            ScanManager.Add(meleeWeapon);
        }
        else
            switch (weapon)
            {
                case WeaponType.Bow:
                    weaponInstance = new MHRBow();
                    break;
                case WeaponType.HeavyBowgun:
                    weaponInstance = new MHRHeavyBowgun();
                    break;
                case WeaponType.LightBowgun:
                    weaponInstance = new MHRLightBowgun();
                    break;
                case WeaponType.None:
                case WeaponType.Greatsword:
                case WeaponType.SwordAndShield:
                case WeaponType.DualBlades:
                case WeaponType.Longsword:
                case WeaponType.Hammer:
                case WeaponType.HuntingHorn:
                case WeaponType.Lance:
                case WeaponType.GunLance:
                case WeaponType.SwitchAxe:
                case WeaponType.ChargeBlade:
                case WeaponType.InsectGlaive:
                default:
                    return;
            }

        Weapon = weaponInstance;

        _weaponId = weapon;
    }

    [ScannableMethod]
    private void GetPlayerAbnormalitiesCleanup()
    {
        long debuffsPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("DEBUFF_ABNORMALITIES_OFFSETS")
        );

        if (!InHuntingZone || debuffsPtr == 0)
            ClearAbnormalities(_abnormalities);
    }

    [ScannableMethod]
    private void GetPlayerEquipmentSkills()
    {
        long armorSkillsPtr = Memory.Read(
            AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_GEAR_SKILLS_ARRAY_OFFSETS")
        );

        MHREquipmentSkillStructure[] armorSkills = Memory.ReadArrayOfPtrs<MHREquipmentSkillStructure>(armorSkillsPtr);

        _armorSkills.Clear();

        foreach (MHREquipmentSkillStructure skill in armorSkills)
            if (skill.Id > 0)
                _armorSkills.Add(skill.Id, skill);
    }

    [ScannableMethod]
    private void GetPlayerSwitchState()
    {
        SwitchScroll = (Scroll)Memory.Deref<int>(
            AddressMap.GetAbsolute("LOCAL_PLAYER_DATA_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_SWITCH_SCROLL_OFFSETS")
        );
    }

    [ScannableMethod]
    private void GetConsumableAbnormalities()
    {
        if (!InHuntingZone)
            return;

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

            if (schema.IsInfinite)
                abnormality.Timer = abnormSubId == schema.WithValue ? AbnormalityData.TIMER_MULTIPLIER : 0;
            else if (abnormSubId == schema.WithValue)
                abnormality = Process.Memory.Read<MHRConsumableStructure>(consumableBuffs + schema.Offset);

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRConsumableAbnormality, MHRConsumableStructure>(
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
        // Only scan in hunting zone due to invalid pointer when not in a hunting zone...
        if (!InHuntingZone)
            return;

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

            bool isConditionValid = schema.CompareOperator switch
            {
                AbnormalityCompareType.WithValue => abnormSubId == schema.WithValue,
                AbnormalityCompareType.WithValueNot => abnormSubId != schema.WithValueNot,
                _ => false
            };

            MHRDebuffStructure abnormality = new();

            // Only read memory if the required sub Id is the required one for this abnormality
            if (schema.IsInfinite)
                abnormality.Timer = isConditionValid ? AbnormalityData.TIMER_MULTIPLIER : 0;
            else if (isConditionValid)
                abnormality = Process.Memory.Read<MHRDebuffStructure>(debuffsPtr + schema.Offset);

            abnormality.Timer /= AbnormalityData.TIMER_MULTIPLIER;

            HandleAbnormality<MHRDebuffAbnormality, MHRDebuffStructure>(
                _abnormalities,
                schema,
                abnormality.Timer,
                abnormality
            );
        }
    }

    [ScannableMethod]
    private void GetSessionPlayers()
    {
        int questState = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("QUEST_ADDRESS"),
            AddressMap.Get<int[]>("QUEST_STATUS_OFFSETS")
        );

        if (questState.IsQuestFinished())
            return;

        // Only scan party members when the player is not in the quest end screen
        if (!questState.IsInQuest() && !StageId.IsTrainingRoom())
        {
            _party.Clear();
            return;
        }

        long playersArrayPtr = Process.Memory.Deref<long>(
            AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
            AddressMap.Get<int[]>("SOS_SESSION_PLAYER_OFFSETS")
        );

        long playersWeaponPtr = Process.Memory.Deref<long>(
            AddressMap.GetAbsolute("SESSION_PLAYERS_ADDRESS"),
            AddressMap.Get<int[]>("SESSION_PLAYER_OFFSETS")
        );

        (int index, bool isValid, MHRCharacterData data)[] sessionPlayersArray = Process.Memory.Read<long>(playersArrayPtr + 0x20, 4)
            .Select(pointer => (isValid: pointer != 0, pointer))
            .Select((player, index) => (index, player.isValid, Process.Memory.Read<MHRCharacterData>(player.pointer)))
            .ToArray();

        bool isSos = sessionPlayersArray.Any(player => player.isValid);

        if (!isSos)
        {
            playersArrayPtr = Process.Memory.Deref<long>(
                AddressMap.GetAbsolute("CHARACTER_ADDRESS"),
                AddressMap.Get<int[]>("CHARACTER_INFO_OFFSETS")
            );

            sessionPlayersArray = Process.Memory.Read<long>(playersArrayPtr + 0x20, 4)
                                                 .Select(pointer => (isValid: pointer != 0, pointer))
                                                 .Select((player, index) => (index, player.isValid, Process.Memory.Read<MHRCharacterData>(player.pointer)))
                                                 .ToArray();
        }

        bool isOnlineSession = sessionPlayersArray.Any(player => player.isValid);

        long[] playerWeaponsPtr = Process.Memory.Read<long>(playersWeaponPtr + 0x20, 4);

        // In case player DC'd
        if (!isOnlineSession)
        {
            if (string.IsNullOrEmpty(Name))
                return;

            _party.Update(new MHRPartyMemberData()
            {
                Index = 0,
                Name = Name,
                HighRank = HighRank,
                MasterRank = MasterRank,
                WeaponId = _weaponId,
                IsMyself = true
            });
            return;
        }

        foreach ((int index, bool isValid, MHRCharacterData data) in sessionPlayersArray)
        {
            long weaponPtr = playerWeaponsPtr[index];
            string name = Process.Memory.Read(data.NamePointer + 0x14, 32, Encoding.Unicode);

            if (weaponPtr == 0 && !isValid)
            {
                _party.Remove(index);
                continue;
            }

            Weapon weapon = Process.Memory.Read<int>(weaponPtr + 0x134).ToWeaponId();

            var memberData = new MHRPartyMemberData
            {
                Index = index,
                Name = name,
                HighRank = data.HighRank,
                MasterRank = data.MasterRank,
                WeaponId = weapon,
                IsMyself = name == Name
            };

            _party.Update(memberData);
        }
    }

    [ScannableMethod]
    private void GetPlayerSongAbnormalities()
    {
        if (!InHuntingZone)
            return;

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

    [ScannableMethod]
    private void GetPlayerStatus()
    {
        if (!InHuntingZone)
            return;

        long playerHudPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("UI_ADDRESS"),
            AddressMap.Get<int[]>("PLAYER_HUD_OFFSETS")
        );

        if (playerHudPtr.IsNullPointer())
            return;

        MHRPlayerHudStructure playerHud = Process.Memory.Read<MHRPlayerHudStructure>(playerHudPtr);

        MHRPetalaceStatsStructure? petalace = GetEquippedPetalaceStats();

        if (petalace is null)
            return;

        var healthData = new HealthData
        {
            MaxHealth = playerHud.MaxHealth,
            Health = playerHud.Health,
            RecoverableHealth = playerHud.RecoverableHealth,
            MaxPossibleHealth = petalace.Value.CalculateMaxPlayerHealth(),
            Heal = playerHud.Heal
        };

        // For when Berserk skill is active
        if (_armorSkills.ContainsKey(137) && SwitchScroll == Scroll.Blue)
            healthData = healthData with
            {
                Health = 0,
                RecoverableHealth = healthData.Health,
                Heal = healthData.Heal > healthData.Health ? healthData.Heal : 0
            };

        _health.Update(healthData);

        var staminaData = new StaminaData
        {
            MaxStamina = playerHud.MaxStamina,
            Stamina = playerHud.Stamina,
            MaxRecoverableStamina = playerHud.MaxExtendableStamina,
            MaxPossibleStamina = petalace.Value.CalculateMaxPlayerStamina(),
        };

        _stamina.Update(staminaData);
    }

    [ScannableMethod(typeof(MHRWirebugData))]
    private void GetPlayerWirebugs()
    {
        long wirebugsArrayPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("WIREBUG_DATA_OFFSETS")
        );

        if (wirebugsArrayPtr.IsNullPointer())
        {
            this.Dispatch(_onWirebugsRefresh, Array.Empty<MHRWirebug>());
            return;
        }

        int wirebugsArrayLength = Math.Min(Wirebugs.Length, Process.Memory.Read<int>(wirebugsArrayPtr + 0x1C));
        long[] wirebugsPtrs = Process.Memory.Read<long>(wirebugsArrayPtr + 0x20, (uint)wirebugsArrayLength);

        bool shouldDispatchEvent = false;
        for (int i = 0; i < wirebugsArrayLength; i++)
        {
            long wirebugPtr = wirebugsPtrs[i];

            var data = new MHRWirebugData
            {
                IsBlocked = Process.Memory.Deref<int>(
                    AddressMap.GetAbsolute("UI_ADDRESS"),
                    AddressMap.Get<int[]>("IS_WIREBUG_BLOCKED_OFFSETS")
                ) != 0,
                Structure = Process.Memory.Read<MHRWirebugStructure>(wirebugPtr)
            };
            data.Structure.Cooldown /= AbnormalityData.TIMER_MULTIPLIER;
            data.Structure.MaxCooldown /= AbnormalityData.TIMER_MULTIPLIER;

            if (wirebugPtr != Wirebugs[i].Address)
            {
                shouldDispatchEvent = true;
                Wirebugs[i].Address = wirebugPtr;
            }

            IUpdatable<MHRWirebugData> wirebug = Wirebugs[i];
            wirebug.Update(data);
        }

        // Update last Wirebug with extra data
        MHRWirebugExtrasStructure extraData = Process.Memory.Deref<MHRWirebugExtrasStructure>(
            AddressMap.GetAbsolute("ABNORMALITIES_ADDRESS"),
            AddressMap.Get<int[]>("WIREBUG_EXTRA_DATA_OFFSETS")
        );
        extraData.Timer /= AbnormalityData.TIMER_MULTIPLIER;
        IUpdatable<MHRWirebugExtrasStructure> lastWirebug = Wirebugs[^1];
        lastWirebug.Update(extraData);

        if (shouldDispatchEvent)
            this.Dispatch(_onWirebugsRefresh, Wirebugs);
    }

    [ScannableMethod(typeof(MHRSubmarineData))]
    private void GetArgosy()
    {

        long argosyAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("ARGOSY_ADDRESS"),
            AddressMap.Get<int[]>("ARGOSY_OFFSETS")
        );

        int submarineArrayLength = Process.Memory.Read<int>(argosyAddress + 0x1C);
        var submarines = new MHRSubmarineData[submarineArrayLength];

        long[] submarinePtrs = Process.Memory.Read<long>(argosyAddress + 0x20, (uint)submarineArrayLength);

        // Read submarines data
        for (int i = 0; i < submarineArrayLength; i++)
        {
            ref long submarinePtr = ref submarinePtrs[i];
            MHRSubmarineStructure data = Process.Memory.Read<MHRSubmarineStructure>(submarinePtr);
            submarines[i].Data = data;
        }

        // Read submarine items array data
        for (int i = 0; i < submarines.Length; i++)
        {
            ref MHRSubmarineData submarine = ref submarines[i];

            int itemsArrayLength = Process.Memory.Read<int>(submarine.Data.ItemArrayPtr + 0x1C);
            long[] itemsPtr = Process.Memory.Read<long>(submarine.Data.ItemArrayPtr + 0x20, (uint)itemsArrayLength)
                                             .Select(ptr => Process.Memory.Read<long>(ptr + 0x20))
                                             .ToArray();
            var items = new MHRSubmarineItemStructure[itemsArrayLength];

            for (int j = 0; j < itemsArrayLength; j++)
                items[j] = Process.Memory.Read<MHRSubmarineItemStructure>(itemsPtr[j]);

            submarine.Items = items;
        }

        for (int i = 0; i < Math.Min(Argosy.Submarines.Length, submarineArrayLength); i++)
        {
            IUpdatable<MHRSubmarineData> localData = Argosy.Submarines[i];
            localData.Update(submarines[i]);
        }
    }

    [ScannableMethod(typeof(MHRCohootStructure))]
    private void GetCohoot()
    {
        MHRCohootStructure cohoot = Process.Memory.Deref<MHRCohootStructure>(
            AddressMap.GetAbsolute("COHOOT_ADDRESS"),
            AddressMap.Get<int[]>("COHOOT_COUNT_OFFSETS")
        );

        Next(ref cohoot);

        IUpdatable<MHRCohootStructure> updatable = Cohoot;
        updatable.Update(cohoot);
    }

    [ScannableMethod(typeof(MHRTrainingDojoData))]
    private void GetTrainingDojo()
    {
        int[] staticTrainingData = Process.Memory.Read<int>(AddressMap.GetAbsolute("DATA_TRAINING_DOJO_ROUNDS_LEFT"), 5);
        MHRTrainingDojoData data = new()
        {
            Rounds = staticTrainingData[0],
            MaxRounds = staticTrainingData[1],
            Boosts = staticTrainingData[3],
            MaxBoosts = staticTrainingData[4],
            Buddies = new MHRBuddyData[6]
        };

        long trainingDojo = Process.Memory.Read(
            AddressMap.GetAbsolute("TRAINING_DOJO_ADDRESS"),
            AddressMap.Get<int[]>("TRAINING_DOJO_OFFSETS")
        );

        data.BuddiesCount = Process.Memory.Read<int>(trainingDojo + 0x18);

        long trainingDojoBuddyArray = Process.Memory.Read(
            AddressMap.GetAbsolute("TRAINING_DOJO_ADDRESS"),
            AddressMap.Get<int[]>("TRAINING_DOJO_BUDDY_ARRAY_OFFSETS")
        );

        long[] buddyPtrs = Process.Memory.Read<long>(trainingDojoBuddyArray + 0x20, 6);

        for (int i = 0; i < data.BuddiesCount; i++)
            data.Buddies[i] = DerefBuddyData(buddyPtrs[i]);

        Next(ref data);

        TrainingDojo.Update(data);
    }

    [ScannableMethod(typeof(MHRMeowmasterData))]
    private void GetMeowcenaries()
    {
        long meowmastersAddress = Process.Memory.Read(
            AddressMap.GetAbsolute("MEOWMASTERS_ADDRESS"),
            AddressMap.Get<int[]>("MEOWMASTERS_OFFSETS")
        );

        MHRMeowmasterStructure structure = Process.Memory.Read<MHRMeowmasterStructure>(meowmastersAddress);
        MHRMeowmasterData data = new()
        {
            IsDeployed = structure.IsDeployed,
            IsLagniappleActive = structure.IsLagniappleActive,
            BuddiesCount = Process.Memory.Read<int>(structure.BuddiesPointer + 0x18),
            CurrentStep = structure.CurrentStep,
            MaxStep = 5
        };

        Next(ref data);

        IUpdatable<MHRMeowmasterData> updatable = Meowmasters;
        updatable.Update(data);
    }

    [ScannableMethod]
    private void GetPartyData()
    {
        long partyArrayPtr = Process.Memory.Read(
            AddressMap.GetAbsolute("SESSION_PLAYERS_ADDRESS"),
            AddressMap.Get<int[]>("SESSION_PLAYERS_ARRAY_OFFSETS")
        );

        long[] playerAddresses = Process.Memory.Read<long>(partyArrayPtr + 0x20, (uint)_party.MaxSize);

        int membersCount = playerAddresses.Count(address => address != 0x0);

        _party.SetSize(membersCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private MHRBuddyData DerefBuddyData(long buddyPtr)
    {
        long namePtr = Process.Memory.ReadPtr(buddyPtr, AddressMap.Get<int[]>("BUDDY_NAME_OFFSETS"));
        long levelPtr = Process.Memory.ReadPtr(buddyPtr, AddressMap.Get<int[]>("BUDDY_LEVEL_OFFSETS"));
        int nameLength = Process.Memory.Read<int>(namePtr + 0x10);

        MHRBuddyData data = new()
        {
            Name = Process.Memory.Read(namePtr + 0x14, (uint)nameLength * 2, Encoding.Unicode),
            Level = Process.Memory.Read<int>(levelPtr + 0x24)
        };

        return data;
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

    private MHRPetalaceStatsStructure? GetEquippedPetalaceStats()
    {
        long petalaceArray = Process.Memory.Read(
            AddressMap.GetAbsolute("GEAR_ADDRESS"),
            AddressMap.Get<int[]>("PETALACES_ARRAY_OFFSETS")
        );

        if (petalaceArray == 0)
            return null;

        int selectedPetalaceId = Process.Memory.Deref<int>(
            AddressMap.GetAbsolute("PLAYER_GEAR_ADDRESS"),
            AddressMap.Get<int[]>("SELECTED_PETALACE_OFFSETS")
        ) & 0x0000FFFF;

        uint petalaceArrayLength = Process.Memory.Read<uint>(petalaceArray + 0x1C);
        long[] petalacePtrs = Process.Memory.Read<long>(petalaceArray + 0x20, petalaceArrayLength);

        MHRPetalaceStructure structure = Process.Memory.Read<MHRPetalaceStructure>(
            petalacePtrs[selectedPetalaceId % petalacePtrs.Length]
        );

        MHRPetalaceDataStructure data = Process.Memory.Read<MHRPetalaceDataStructure>(structure.Data);

        return Process.Memory.Read<MHRPetalaceStatsStructure>(data.Stats);
    }

    internal void UpdatePartyMembersDamage(EntityDamageData[] entities)
    {
        foreach (EntityDamageData entity in entities)
            _party.Update(entity);
    }

    public override void Dispose()
    {
        Wirebugs.DisposeAll();
        Argosy.Dispose();
        TrainingDojo.Dispose();
        Meowmasters.Dispose();
        Cohoot.Dispose();
        _onWirebugsRefresh.Dispose();
        base.Dispose();
    }
}
