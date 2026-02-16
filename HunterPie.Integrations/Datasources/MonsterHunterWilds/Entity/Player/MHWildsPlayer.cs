using HunterPie.Core.Address.Map;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Client.Localization;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Memory.Types;
using HunterPie.Core.Domain.Process.Entity;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Data.Definitions;
using HunterPie.Core.Game.Data.Repository;
using HunterPie.Core.Game.Entity;
using HunterPie.Core.Game.Entity.Party;
using HunterPie.Core.Game.Entity.Player;
using HunterPie.Core.Game.Entity.Player.Classes;
using HunterPie.Core.Game.Entity.Player.Vitals;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Events;
using HunterPie.Core.Game.Services;
using HunterPie.Core.Scan.Service;
using HunterPie.Core.Utils;
using HunterPie.Integrations.Datasources.Common.Entity.Player;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Utils;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Abnormality;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Party.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Player;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Quest;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Save;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Definitions.Types;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Abnormalities.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Activities.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Crypto;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Enemy;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Party.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Data;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player.Weapons;
using HunterPie.Integrations.Datasources.MonsterHunterWilds.Utils;
using WeaponType = HunterPie.Core.Game.Enums.Weapon;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Entity.Player;

public sealed class MHWildsPlayer(
    IGameProcess process,
    IScanService scanService,
    MHWildsMonsterTargetKeyManager monsterTargetKeyManager,
    MHWildsCryptoService cryptoService,
    ILocalizationRepository localizationRepository) : CommonPlayer(process, scanService)
{
    private const int MAX_DAMAGE_HISTORY_SIZE = 100;
    private static readonly Lazy<AbnormalityDefinition[]> ConsumableDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.CONSUMABLES
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> SongsDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.SONGS
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> PalicoSongsDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.ORCHESTRA
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> DebuffDefinitions = new(static () =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.DEBUFFS
        )
    );
    private static readonly Lazy<AbnormalityDefinition[]> SkillDefinitions = new(() =>
        AbnormalityRepository.FindAllAbnormalitiesBy(
            game: GameType.Wilds,
            category: AbnormalityGroup.SKILLS
        )
    );
    private static readonly Lazy<int> DebuffIndexMax = new(static () => DebuffDefinitions.Value.Max(it => it.Index));

    private readonly MHWildsMonsterTargetKeyManager _monsterTargetKeyManager = monsterTargetKeyManager;
    private readonly MHWildsCryptoService _cryptoService = cryptoService;
    private readonly ILocalizationRepository _localizationRepository = localizationRepository;

    private nint _address;
    private nint _saveAddress;

    private string _name = string.Empty;
    public override string Name
    {
        get => _name;
        protected set
        {
            if (value == _name)
                return;

            _name = value;

            this.Dispatch(
                value != ""
                    ? _onLogin
                    : _onLogout
            );
        }
    }

    private int _highRank;
    public override int HighRank
    {
        get => _highRank;
        protected set
        {
            if (value == _highRank)
                return;

            _highRank = value;
            this.Dispatch(
                toDispatch: _onLevelChange,
                data: new LevelChangeEventArgs(this)
            );
        }
    }

    private int _masterRank;
    public override int MasterRank
    {
        get => _masterRank;
        protected set
        {
            if (value == _masterRank)
                return;

            _masterRank = value;
            this.Dispatch(
                toDispatch: _onLevelChange,
                data: new LevelChangeEventArgs(this)
            );
        }
    }

    private int _stageId = -1;
    public override int StageId
    {
        get => _stageId;
        protected set
        {
            if (value == _stageId)
                return;

            _stageId = value;
            this.Dispatch(
                toDispatch: _onStageUpdate,
                data: EventArgs.Empty
            );
        }
    }

    private bool _inHuntingZone;
    public override bool InHuntingZone => _inHuntingZone;

    private readonly MHWildsParty _party = new();
    public override IParty Party => _party;

    private readonly Dictionary<string, IAbnormality> _abnormalities = new();
    public override IReadOnlyCollection<IAbnormality> Abnormalities => _abnormalities.Values;

    public override IHealthComponent? Health { get; }
    public override IStaminaComponent? Stamina { get; }

    private readonly MHWildsPlayerStatus _status = new();
    public override IPlayerStatus Status => _status;

    private IWeapon _weapon = new MHWildsWeapon(WeaponType.Greatsword);
    public override IWeapon Weapon
    {
        get => _weapon;
        protected set
        {
            if (_weapon == value)
                return;

            IWeapon oldWeapon = _weapon;
            _weapon = value;
            this.Dispatch(
                toDispatch: _onWeaponChange,
                data: new WeaponChangeEventArgs(oldWeapon, value)
            );
        }
    }

    public MHWildsMaterialRetrieval MaterialRetrieval { get; } = new();

    public MHWildsSupportShip SupportShip { get; } = new();

    public MHWildsIngredientsCenter IngredientsCenter { get; } = new();

    private readonly MHWildsSpecializedTool[] _tools = { new(), new() };
    public IReadOnlyCollection<ISpecializedTool> Tools => _tools;

    [ScannableMethod]
    internal async Task GetBasicDataAsync()
    {
        _address = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::Local")
        );

        MHWildsPlayerContext context = await Memory.DerefPtrAsync<MHWildsPlayerContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Context")
        );

        int hunterRank = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::SaveManager"),
            offsets: AddressMap.GetOffsets("Save::Player::HunterRank")
        );

        Name = await Memory.ReadStringSafeAsync(context.NamePointer, size: 64);
        HighRank = hunterRank;
        Position = context.Position.ToVector3();
    }

    [ScannableMethod]
    internal async Task GetStageAsync()
    {
        MHWildsStageContext context = await Memory.DerefPtrAsync<MHWildsStageContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Stage")
        );

        bool wasInHuntingZone = _inHuntingZone;
        int pauseState = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::PauseManager"),
            offsets: AddressMap.GetOffsets("Game::PauseState")
        );
        bool isLoading = (pauseState & 6) > 0;
        bool isInSafeAreaForced = context.StageId is 14 or 12;

        _inHuntingZone = context is { IsSafeZone: false, StageId: >= 0 } &&
            !isInSafeAreaForced &&
            !isLoading &&
            !string.IsNullOrEmpty(Name);

        if (wasInHuntingZone && !_inHuntingZone)
            this.Dispatch(_onVillageEnter);
        else if (!wasInHuntingZone && _inHuntingZone)
            this.Dispatch(_onVillageLeave);

        if (isLoading)
            StageId = -1;

        if (context.StageId != StageId && isLoading)
            return;

        StageId = context.StageId;
    }

    [ScannableMethod]
    internal async Task GetWeaponAsync()
    {
        MHWildsPlayerGearContext context = await Memory.DerefPtrAsync<MHWildsPlayerGearContext>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Gear")
        );

        if (Weapon.Id != context.WeaponId)
            Weapon = new MHWildsWeapon(context.WeaponId);
    }

    [ScannableMethod]
    internal async Task GetStatusAsync()
    {
        UpdatePlayerStatus status = await GetPlayerStatusAsync(_address);

        _status.Update(status);
    }

    [ScannableMethod]
    internal async Task GetPartyAsync()
    {
        if (StageId < 0)
            return;

        nint partyLimitedArrayPointer = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::Party")
        );
        nint partyMemberIndexesArrayPointer = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::Quest::PlayerIndexes")
        );
        MHWildsLimitedArray partyArrayPointer = await Memory.ReadAsync<MHWildsLimitedArray>(partyLimitedArrayPointer);
        MHWildsPartyArray partyArray = await Memory.ReadAsync<MHWildsPartyArray>(partyArrayPointer.Elements);
        MHWildsPartyMemberIndex[] partyIndexesArray = await Memory.ReadArraySafeAsync<MHWildsPartyMemberIndex>(
            address: partyMemberIndexesArrayPointer,
            count: 4
        );

        if (partyArray.Capacity != 4
            || partyArray.Members is not { Length: 4 }
            || partyIndexesArray is not { Length: 4 })
        {
            _party.Clear();
            return;
        }

        int membersCount = Math.Max(1, partyArrayPointer.Length);

        if (membersCount > 4)
            return;

        nint networkPartyMemberArray = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::NetworkManager"),
            offsets: AddressMap.GetOffsets("Network::Party")
        );

        var membersData = new UpdatePartyMember[membersCount];
        for (int i = 0; i < membersCount; i++)
        {
            int playerIndex = partyArray.Members[i];
            nint playerPointer = await Memory.ReadAsync(
                address: AddressMap.GetAbsolute("Game::PlayerManager"),
                offsets: AddressMap.GetOffsets("Player::List")
            );
            playerPointer += playerIndex * sizeof(long);
            playerPointer = await Memory.ReadAsync<nint>(playerPointer);

            MHWildsPlayerBase playerBase = await Memory.ReadAsync<MHWildsPlayerBase>(
                address: playerPointer
            );

            if (!playerBase.IsReady() && !_party.Contains(playerBase.BasePointer))
                continue;

            MHWildsPlayerContext playerContext = await Memory.DerefPtrAsync<MHWildsPlayerContext>(
                address: playerBase.BasePointer,
                offsets: AddressMap.GetOffsets("Player::Context")
            );
            MHWildsPlayerNetworkInfo networkInfo = await Memory.ReadAsync<MHWildsPlayerNetworkInfo>(playerContext.NetworkInfo);

            if (networkInfo.QuestIndex >= partyIndexesArray.Length || networkInfo.QuestIndex < 0)
                continue;

            int realPartyIndex = partyIndexesArray[networkInfo.QuestIndex].Index;

            string name = await GetPlayerNameAsync(playerContext);

            if (string.IsNullOrEmpty(name))
                continue;

            Task<float> damageDefer = GetDamageByPlayerAsync(playerBase.BasePointer, realPartyIndex);

            bool isMyself = playerIndex == 0;
            var data = new UpdatePartyMember
            {
                IsValid = true,
                Id = playerBase.BasePointer,
                Index = realPartyIndex,
                IsMyself = isMyself,
                Name = name,
                Weapon = isMyself ? Weapon.Id : await GetPlayerWeaponAsync(playerBase.BasePointer),
                Damage = await damageDefer,
                HunterRank = isMyself
                    ? HighRank
                    : await Memory.DerefAsync<int>(
                        address: networkPartyMemberArray + (realPartyIndex * sizeof(long)),
                        offsets: AddressMap.GetOffsets("Network::Party::Member::HunterRank")
                    ),
                Status = await GetPlayerStatusAsync(playerBase.BasePointer),
            };

            membersData[i] = data;
        }

        IEnumerable<UpdatePartyMember> npcPartyMembers = await GetNpcPartyMembersAsync(playerCount: membersData.Length);
        membersData = membersData.Concat(npcPartyMembers)
            .ToArray();

        _party.Update(new UpdateParty
        {
            Players = membersData.Where(it => it.IsValid)
                .Select(it => it.Id)
                .ToArray()
        });

        membersData.ForEach(_party.Update);
    }

    private async Task<UpdatePlayerStatus> GetPlayerStatusAsync(nint address)
    {
        MHWildsAffinityStatus affinityContext = await Memory.DerefPtrAsync<MHWildsAffinityStatus>(
            address: address,
            offsets: AddressMap.GetOffsets("Player::Status::Affinity")
        );
        MHWildsDamageStatus damageContext = await Memory.DerefPtrAsync<MHWildsDamageStatus>(
            address: address,
            offsets: AddressMap.GetOffsets("Player::Status::Damage")
        );

        MHWildsEncryptedFloat encryptedCritRate = await affinityContext.CurrentCriticalRate.Deref(Memory);
        MHWildsEncryptedFloat encryptedRawDamage = await damageContext.CurrentRawAttack.Deref(Memory);
        MHWildsEncryptedFloat encryptedElementalDamage = await damageContext.CurrentElementalAttack.Deref(Memory);

        return new UpdatePlayerStatus
        {
            Affinity = await _cryptoService.DecryptFloatAsync(encryptedCritRate),
            RawDamage = await _cryptoService.DecryptFloatAsync(encryptedRawDamage),
            ElementalDamage = Math.Round(await _cryptoService.DecryptFloatAsync(encryptedElementalDamage) * 10),
        };
    }

    private async Task<IEnumerable<UpdatePartyMember>> GetNpcPartyMembersAsync(int playerCount)
    {
        List<UpdatePartyMember> members = new();
        Ref<nint> npcArrayPointer = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::NpcManager"),
            offsets: AddressMap.GetOffsets("Npcs::Party")
        );
        IAsyncEnumerable<MHWildsNpcPartyMember> npcPointers = Memory.ReadArrayOfPtrsSafeAsync<MHWildsNpcPartyMember>(
            address: await npcArrayPointer.Deref(Memory),
            size: 6
        );

        int index = 0;
        await foreach (MHWildsNpcPartyMember npc in npcPointers)
        {
            if (!npc.IsInParty() || npc.IsHandler())
                continue;

            nint damageHistoryPointer = await Memory.ReadPtrAsync(
                address: npc.ContextPointer,
                offsets: AddressMap.GetOffsets("Npc::DamageHistory")
            );
            Task<float> damageDefer = GetHistoricalPlayerDamage(damageHistoryPointer);
            MHWildsNpcCreation creationParams = await npc.CreationParams.Deref(Memory);

            members.Add(new UpdatePartyMember
            {
                Id = npc.ContextPointer,
                Name = creationParams.Id.ToNpcName(_localizationRepository),
                IsNpc = true,
                IsValid = true,
                Damage = await damageDefer,
                HunterRank = 100,
                IsMyself = false,
                Index = playerCount + index,
                Weapon = creationParams.Weapon,
            });
            index++;
        }

        return members;
    }

    private async Task<float> GetDamageByPlayerAsync(nint address, int index)
    {
        bool isLocalPlayer = address == _address;

        float syncedDamage = isLocalPlayer switch
        {
            true => await GetQuestSyncedLocalPlayerDamage(),
            _ => await GetQuestSyncedRemotePlayerDamage(index)
        };

        if (syncedDamage > 0)
            return syncedDamage;

        nint damageHistoryPointer = await Memory.ReadPtrAsync(
            address: address,
            offsets: AddressMap.GetOffsets("Player::DamageHistory")
        );
        return await GetHistoricalPlayerDamage(damageHistoryPointer);
    }

    private async Task<float> GetHistoricalPlayerDamage(nint damageHistoryPointer)
    {
        MHWildsDamageHistory damageHistory = await Memory.ReadAsync<MHWildsDamageHistory>(damageHistoryPointer);

        if (damageHistory.Size <= 0)
            return 0;

        int offset = 0x20;
        int damageHistorySafeSize = damageHistory.Size;
        // The history array grows infinitely and stores the damage done to each monster individually
        // we don't need to actually read every monster damage as some of the monsters might not even be in the map anymore
        // This also handles invalid memory addresses that sometimes makes HunterPie read insanely high lengths
        if (damageHistorySafeSize > MAX_DAMAGE_HISTORY_SIZE)
        {
            offset += sizeof(long) * (damageHistorySafeSize - MAX_DAMAGE_HISTORY_SIZE);
            damageHistorySafeSize = MAX_DAMAGE_HISTORY_SIZE;
        }

        nint[] damagePointers = await Memory.ReadAsync<nint>(
            address: damageHistory.Elements + offset,
            count: damageHistorySafeSize
        );
        IAsyncEnumerable<MHWildsTargetDamage> damageEnumerable = damagePointers.AsParallel()
            .Select(async it => await Memory.ReadAsync<MHWildsTargetDamage>(it))
            .ToAsyncEnumerable();

        float totalDamage = 0;

        await foreach (MHWildsTargetDamage targetDamage in damageEnumerable)
        {
            bool hasQuestTargets = _monsterTargetKeyManager.HasQuestTargets();
            bool isInExpedition = !hasQuestTargets && _monsterTargetKeyManager.IsMonster(targetDamage.TargetKey);
            bool isInQuest = hasQuestTargets && _monsterTargetKeyManager.IsQuestTarget(targetDamage.TargetKey);

            if (!isInQuest && !isInExpedition)
                continue;

            totalDamage += targetDamage.Damage;
        }

        return totalDamage;
    }

    private async Task<float> GetQuestSyncedLocalPlayerDamage()
    {
        nint damagePointer = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::LocalPlayer::Damage")
        );

        return await Memory.ReadAsync<float>(damagePointer);
    }

    private async Task<float> GetQuestSyncedRemotePlayerDamage(int index)
    {
        nint damagePointer = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::RemotePlayer::Damage")
        );

        return await Memory.ReadAsync<float>(damagePointer + (index * 0x78));
    }

    private async Task<string> GetPlayerNameAsync(MHWildsPlayerContext ctx)
    {

        return await Memory.ReadStringSafeAsync(ctx.NamePointer, size: 64);
    }

    private async Task<WeaponType> GetPlayerWeaponAsync(nint address)
    {
        MHWildsPlayerGearContext context = await Memory.DerefPtrAsync<MHWildsPlayerGearContext>(
            address: address,
            offsets: AddressMap.GetOffsets("Player::Gear")
        );

        return context.WeaponId;
    }

    [ScannableMethod]
    internal async Task GetActivitiesAsync()
    {
        int saveIndex = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::SaveManager"),
            offsets: AddressMap.GetOffsets("Save::Index")
        );

        nint saveDataArray = await Memory.ReadAsync(
            address: AddressMap.GetAbsolute("Game::SaveManager"),
            offsets: AddressMap.GetOffsets("Save::Data")
        );
        _saveAddress = await Memory.ReadAsync<nint>(saveDataArray + (sizeof(long) * saveIndex));

        await Task.WhenAll(
            GetMaterialRetrievalAsync(_saveAddress),
            GetSupportShipAsync(_saveAddress),
            GetIngredientsCenterAsync(_saveAddress)
        );
    }

    private async Task GetMaterialRetrievalAsync(nint saveAddress)
    {
        const int maxItems = 16;
        nint sourcesArrayPointer = await Memory.ReadPtrAsync(
            address: saveAddress,
            offsets: AddressMap.GetOffsets("Activities::MaterialRetrieval")
        );

        IAsyncEnumerable<MHWildsMaterialCollector> collectors = Memory.ReadArrayOfPtrsSafeAsync<MHWildsMaterialCollector>(
            address: sourcesArrayPointer,
            count: 12
        );

        await foreach (MHWildsMaterialCollector collector in collectors)
        {
            if (collector.Id.ToMaterialRetrievalSourceType() is not { } type)
                continue;

            List<MHWildsMaterialCollectorItem> items = Memory.ReadArrayOfPtrsSafeAsync<MHWildsMaterialCollectorItem>(
                address: collector.ItemsPointer,
                count: maxItems
            ).Collect();

            var updateData = new UpdateMaterialCollectorData
            {
                Collector = type,
                Count = items.Count(it => it.Count > 0),
                MaxCount = maxItems
            };

            MaterialRetrieval.Update(updateData);
        }
    }

    private async Task GetSupportShipAsync(nint saveAddress)
    {
        MHWildsSupportShipContext context = await Memory.DerefPtrAsync<MHWildsSupportShipContext>(
            address: saveAddress,
            offsets: AddressMap.GetOffsets("Activities::SupportShip")
        );

        SupportShip.Update(context);
    }

    private async Task GetIngredientsCenterAsync(nint saveAddress)
    {
        MHWildsIngredientCenterContext context = await Memory.DerefPtrAsync<MHWildsIngredientCenterContext>(
            address: saveAddress,
            offsets: AddressMap.GetOffsets("Activities::IngredientsCenter")
        );

        IngredientsCenter.Update(context);
    }

    [ScannableMethod]
    public async Task GetSpecializedToolsAsync()
    {
        nint specializedToolPointers = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::SpecializedTools")
        );
        List<MHWildsSpecializedToolContext> contexts = Memory.ReadArrayOfPtrsSafeAsync<MHWildsSpecializedToolContext>(
            address: specializedToolPointers,
            count: 5
        ).Collect();

        int[] toolIds =
        {
            1,
            await GetSecondaryToolId()
        };

        for (int i = 0; i < _tools.Length; i++)
        {
            MHWildsSpecializedTool tool = _tools[i];
            int id = toolIds[i];
            int normalizedId = id - 1;
            var type = id.ToSpecializedToolType();

            if (type is not { } toolType)
                continue;

            MHWildsSpecializedToolContext? context = normalizedId >= 0 && normalizedId < contexts.Count
                ? contexts[normalizedId]
                : null;

            if (context is not { } ctx)
                continue;

            Task<MHWildsEncryptedFloat> timerDefer = ctx.Timer.Deref(Memory);
            Task<MHWildsEncryptedFloat> maxTimerDefer = ctx.MaxTimer.Deref(Memory);
            Task<MHWildsEncryptedFloat> cooldownDefer = ctx.Cooldown.Deref(Memory);
            Task<MHWildsEncryptedFloat> maxCooldownDefer = ctx.MaxCooldown.Deref(Memory);

            var data = new UpdateSpecializedTool
            {
                Type = toolType,
                Timer = await _cryptoService.DecryptFloatAsync(await timerDefer),
                MaxTimer = await _cryptoService.DecryptFloatAsync(await maxTimerDefer),
                Cooldown = await _cryptoService.DecryptFloatAsync(await cooldownDefer),
                MaxCooldown = await _cryptoService.DecryptFloatAsync(await maxCooldownDefer),
                IsTimerActive = ctx.InUse
            };

            tool.Update(data);
        }
    }

    private async Task<int> GetSecondaryToolId()
    {
        bool inArena = await Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::PlayerManager"),
            offsets: AddressMap.GetOffsets("Player::IsInArena")
        ) >= 1;

        if (!inArena)
        {
            MHWildsSaveEquipment equipment = await Memory.DerefPtrAsync<MHWildsSaveEquipment>(
                address: _saveAddress,
                offsets: AddressMap.GetOffsets("Save::SpecializedTool")
            );

            return equipment.SpecializedToolId;
        }


        Task<int> missionIdDefer = Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::MissionId")
        );

        Task<int> selectedLoadoutIndexDefer = Memory.DerefAsync<int>(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::Arena::SelectedLoadout")
        );

        nint arenaDataArrayPointer = await Memory.DerefAsync<nint>(
            address: AddressMap.GetAbsolute("Game::QuestManager"),
            offsets: AddressMap.GetOffsets("Quest::Arena::Data")
        );
        IAsyncEnumerable<MHWildsArenaData> arenaData = Memory.ReadArrayOfPtrsSafeAsync<MHWildsArenaData>(
            address: arenaDataArrayPointer,
            count: 50
        );

        int missionId = await missionIdDefer;
        int selectedLoadoutIndex = await selectedLoadoutIndexDefer;
        await foreach (MHWildsArenaData data in arenaData)
        {
            MHWildsArenaMissionData mission = await data.Mission.Deref(Memory);
            if (mission.Id != missionId)
                continue;

            MHWildsArenaLoadoutData selectedLoadout = await data.Loadouts.ElementAt(Memory, selectedLoadoutIndex);
            MHWildsArenaLoadoutEquipmentData equipment = await selectedLoadout.Equipment.Deref(Memory);

            return equipment.SpecializedToolId;
        }

        return default;
    }

    [ScannableMethod]
    internal Task GetAbnormalitiesCleanUpAsync()
    {
        if (InHuntingZone)
            return Task.CompletedTask;

        ClearAbnormalities(_abnormalities);

        return Task.CompletedTask;
    }

    [ScannableMethod]
    internal async Task GetConsumableAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        ConsumableAbnormalities consumableAbnormalities = await Memory.DerefPtrAsync<ConsumableAbnormalities>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Consumables")
        );

        if (consumableAbnormalities.Raw is not { Length: > 0 })
            return;

        foreach (AbnormalityDefinition definition in ConsumableDefinitions.Value)
        {
            UpdateAbnormalityData data;

            if (!definition.HasMaxTimer)
            {
                int timerOffset = definition.Offset;

                data = new UpdateAbnormalityData
                {
                    ShouldInferMaxTimer = true,
                    Timer = BitConverter.ToSingle(consumableAbnormalities.Raw, timerOffset)
                };
            }
            else
            {
                int maxTimerOffset = definition.Offset;
                int timerOffset = maxTimerOffset + sizeof(float);

                data = new UpdateAbnormalityData
                {
                    MaxTimer = BitConverter.ToSingle(consumableAbnormalities.Raw, maxTimerOffset),
                    Timer = BitConverter.ToSingle(consumableAbnormalities.Raw, timerOffset)
                };
            }

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Consumable)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetSongAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        SongAbnormalities songsAbnormalities = await Memory.DerefPtrAsync<SongAbnormalities>(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Songs")
        );

        AbnormalityDefinition[] songDefinitions = SongsDefinitions.Value;

        float[] songTimers = await Memory.ReadArraySafeAsync<float>(
            address: songsAbnormalities.TimersPointer,
            count: songDefinitions.Length
        );
        float[] songMaxTimers = await Memory.ReadArraySafeAsync<float>(
            address: songsAbnormalities.MaxTimersPointer,
            count: songDefinitions.Length
        );

        for (int i = 0; i < songDefinitions.Length; i++)
        {
            if (i >= songTimers.Length || i >= songMaxTimers.Length)
                break;

            AbnormalityDefinition definition = songDefinitions[i];
            var data = new UpdateAbnormalityData
            {
                Timer = songTimers[i],
                MaxTimer = songMaxTimers[i],
                ShouldInferMaxTimer = definition.HasMaxTimer
            };

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: songDefinitions[i],
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Song)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetPalicoSongAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        nint palicoSongsPointer = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::PalicoSongs")
        );

        AbnormalityDefinition[] definitions = PalicoSongsDefinitions.Value;
        float[] timers = await Memory.ReadArraySafeAsync<float>(
            address: palicoSongsPointer,
            count: definitions.Length
        );

        for (int i = 0; i < timers.Length; i++)
        {
            AbnormalityDefinition definition = definitions[i];
            float timer = timers[i];

            var data = new UpdateAbnormalityData
            {
                ShouldInferMaxTimer = true,
                Timer = timer
            };

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Orchestra)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetSkillAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        nint skillsBasePtr = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Skills")
        );

        AbnormalityDefinition[] definitions = SkillDefinitions.Value;

        foreach (AbnormalityDefinition definition in definitions)
        {
            nint abnormalityPointer = await Memory.ReadAsync<nint>(
                address: skillsBasePtr + definition.Offset
            );

            int dependingValue = definition.DependsOn switch
            {
                0 => definition.WithValue,
                _ => await Memory.ReadAsync<int>(
                    address: abnormalityPointer + definition.DependsOn
                )
            };

            SkillAbnormality abnormality = dependingValue == definition.WithValue
                ? await Memory.ReadAsync<SkillAbnormality>(abnormalityPointer)
                : default;

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: abnormality.Timer,
                newData: new UpdateAbnormalityData
                {
                    Timer = abnormality.Timer,
                    MaxTimer = abnormality.MaxTimer
                },
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Skill)
            );
        }
    }

    [ScannableMethod]
    internal async Task GetDebuffsAbnormalitiesAsync()
    {
        if (!InHuntingZone)
            return;

        nint debuffsComponent = await Memory.ReadPtrAsync(
            address: _address,
            offsets: AddressMap.GetOffsets("Player::Abnormalities::Debuffs")
        );

        AbnormalityDefinition[] definitions = DebuffDefinitions.Value;

        nint[] debuffPointers = await Memory.ReadAsync<nint>(
            address: debuffsComponent + 0x10,
            count: DebuffIndexMax.Value + 1
        );

        foreach (AbnormalityDefinition definition in definitions)
        {
            if (definition.Index >= debuffPointers.Length)
                continue;

            nint debuffPointer = debuffPointers[definition.Index];

            int dependingValue = definition.DependsOn switch
            {
                0 => 0,
                _ => await Memory.ReadAsync<int>(
                    address: debuffPointer + definition.DependsOn
                )
            };

            UpdateAbnormalityData data;

            if (dependingValue != definition.WithValue)
                data = new UpdateAbnormalityData
                {
                    Timer = 0
                };
            else if (definition.IsBuildup)
            {
                DebuffAbnormality abnormality = await Memory.ReadAsync<DebuffAbnormality>(
                    address: debuffPointer
                );
                bool isValidValue = abnormality.BuildUp >= 0 && abnormality.BuildUp <= definition.MaxBuildup;

                data = new UpdateAbnormalityData
                {
                    Timer = isValidValue
                     ? abnormality.BuildUp
                     : 0,
                    MaxTimer = definition.MaxBuildup
                };
            }
            else if (!definition.HasMaxTimer)
            {
                byte isActive = await Memory.ReadAsync<byte>(
                    address: debuffPointer + 0x2F
                );

                data = new UpdateAbnormalityData
                {
                    ShouldInferMaxTimer = true,
                    Timer = isActive == 1
                    ? await Memory.ReadAsync<float>(
                        address: debuffPointer + definition.Offset
                    )
                    : 0
                };
            }
            else
            {
                float[] timers = await Memory.ReadAsync<float>(
                    address: debuffPointer + definition.Offset,
                    count: 2
                );
                byte isActive = await Memory.ReadAsync<byte>(
                    address: debuffPointer + 0x2F
                );

                data = new UpdateAbnormalityData
                {
                    Timer = isActive == 1 ? timers[0] : 0,
                    MaxTimer = timers[1]
                };
            }

            HandleAbnormality(
                abnormalities: _abnormalities,
                schema: definition,
                timer: data.Timer,
                newData: data,
                activator: () => new MHWildsAbnormality(definition, AbnormalityType.Debuff)
            );
        }
    }

    public override void Dispose()
    {
        base.Dispose();
        MaterialRetrieval.Dispose();
        SupportShip.Dispose();
        _tools.DisposeAll();
        _status.Dispose();
    }
}