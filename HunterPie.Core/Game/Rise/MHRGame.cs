using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain;
using HunterPie.Core.Domain.Interfaces;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Client;
using HunterPie.Core.Game.Enums;
using HunterPie.Core.Game.Environment;
using HunterPie.Core.Game.Rise.Entities;
using HunterPie.Core.Game.Rise.Entities.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterPie.Core.Game.Rise
{
#pragma warning disable IDE0051 // Remove unused private members
    public class MHRGame : Scannable, IGame, IEventDispatcher
    {
        public const uint MAXIMUM_MONSTER_ARRAY_SIZE = 5;
        public const int CHAT_MAX_SIZE = 0x40;
        
        private long _lastChatMessagePtr = 0;
        private readonly MHRChat _chat = new MHRChat();

        // TODO: Could probably turn this into a bit mask with 256 bits
        private readonly HashSet<int> MonsterAreas = new() { 5, 201, 202, 203, 204, 205, 207, 209, 210, 211};

        public static readonly int[] VillageStages = { 0, 1, 2, 3, 4, 5 };

        public IPlayer Player { get; }
        public List<IMonster> Monsters { get; } = new();

        public IChat Chat => _chat;

        Dictionary<long, IMonster> monsters = new();

        public event EventHandler<IMonster> OnMonsterSpawn;
        public event EventHandler<IMonster> OnMonsterDespawn;

        public MHRGame(IProcessManager process) : base(process)
        {
            Player = new MHRPlayer(process);

            ScanManager.Add(
                this,
                Player as Scannable
            );
        }

        [ScannableMethod]
        private void ScanChat()
        {
            long chatArrayPtr = _process.Memory.Read(
                AddressMap.GetAbsolute("CHAT_ADDRESS"),
                AddressMap.Get<int[]>("CHAT_OFFSETS")
            );
            long chatArray = _process.Memory.Read<long>(chatArrayPtr);
            int chatCount = _process.Memory.Read<int>(chatArrayPtr + 0x8);

            if (chatCount != CHAT_MAX_SIZE && chatCount <= Chat.Messages.Count)
                return;
            
            //int chatArrayLength = _process.Memory.Read<int>(chatArray + 0x1C);
            long[] chatMessagePtrs = _process.Memory.Read<long>(chatArray + 0x20, (uint)chatCount);

            if (chatCount == CHAT_MAX_SIZE && chatMessagePtrs[chatCount - 1] == _lastChatMessagePtr)
                return;

            for (int i = chatCount % CHAT_MAX_SIZE; i < chatCount; i++)
            {
                long messagePtr = chatMessagePtrs[i];
                
                if (_chat.ConstainsMessage(messagePtr))
                    continue;

                MHRChatMessage message = DerefChatMessage(messagePtr);
                _chat.AddMessage(messagePtr, message);
            }

            _lastChatMessagePtr = chatMessagePtrs[chatCount - 1];
        }

        [ScannableMethod]
        private void ScanChatUi()
        {
            bool isChatOpen = _process.Memory.Deref<byte>(
                AddressMap.GetAbsolute("CHAT_UI_ADDRESS"),
                AddressMap.Get<int[]>("CHAT_UI_OFFSETS")
            ) == 1;

            _chat.IsChatOpen = isChatOpen;
        }

        [ScannableMethod]
        private void ScanMonstersArray()
        {
            // Only scans for monsters in hunting areas
            if (!MonsterAreas.Contains(Player.StageId))
            {
                if (monsters.Keys.Count > 0)
                    foreach (long mAddress in monsters.Keys)
                        HandleMonsterDespawn(mAddress);

                return;
            }

            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("MONSTERS_ADDRESS"),
                AddressMap.Get<int[]>("MONSTER_LIST_OFFSETS")
            );

            uint monsterArraySize = _process.Memory.Read<uint>(address - 0x8);
            HashSet<long> monsterAddresses = _process.Memory.Read<long>(address + 0x20, Math.Max(MAXIMUM_MONSTER_ARRAY_SIZE, monsterArraySize))
                .ToHashSet();

            long[] toDespawn = monsters.Keys.Where(address => !monsterAddresses.Contains(address))
                .ToArray();

            foreach (long mAddress in toDespawn)
                HandleMonsterDespawn(mAddress);

            long[] toSpawn = monsterAddresses.Where(address => !monsters.ContainsKey(address))
                .ToArray();

            foreach (long mAddress in toSpawn)
                HandleMonsterSpawn(mAddress);

        }

        private void HandleMonsterSpawn(long monsterAddress)
        {
            if (monsterAddress == 0 || monsters.ContainsKey(monsterAddress))
                return;

            IMonster monster = new MHRMonster(_process, monsterAddress);
            monsters.Add(monsterAddress, monster);
            Monsters.Add(monster);
            ScanManager.Add(monster as Scannable);

            this.Dispatch(OnMonsterSpawn, monster);
        }

        private void HandleMonsterDespawn(long address)
        {
            IMonster monster = monsters[address]; 
            monsters.Remove(address);
            Monsters.Remove(monster);
            ScanManager.Remove(monster as Scannable);

            this.Dispatch(OnMonsterDespawn, monster);
        }

        #region Chat helpers
        private MHRChatMessage DerefChatMessage(long messagePtr)
        {
            int messageType = _process.Memory.Read<int>(messagePtr + 0x10);

            return messageType switch
            {
                0x0 => DerefNormalChatMessage(messagePtr),
                0x1 => DerefAutoChatMessage(messagePtr),
                _ => DerefUnknownTypeMessage(messagePtr)
            };
        }

        private MHRChatMessage DerefNormalChatMessage(long messagePtr)
        {
            long messageAuthorPtr = _process.Memory.Read<long>(messagePtr + 0x28);
            long messageStringPtr = _process.Memory.Read<long>(messagePtr + 0x58);

            int messageStringLength = _process.Memory.Read<int>(messageStringPtr + 0x10);
            int messageAuthorLength = _process.Memory.Read<int>(messageAuthorPtr + 0x10);

            string messageString = _process.Memory.Read(messageStringPtr + 0x14, (uint)messageStringLength * 2, Encoding.Unicode);
            string messageAuthor = _process.Memory.Read(messageAuthorPtr + 0x10, (uint) messageAuthorLength * 2, Encoding.Unicode);

            return new()
            {
                Message = messageString,
                Author = messageAuthor,
                Type = AuthorType.Player1
            };
        }

        private MHRChatMessage DerefAutoChatMessage(long messagePtr)
        {
            long messageAuthorPtr = _process.Memory.Read<long>(messagePtr + 0x28);
            int messageAuthorLength = _process.Memory.Read<int>(messageAuthorPtr + 0x10);
            string messageAuthor = _process.Memory.Read(messageAuthorPtr + 0x10, (uint)messageAuthorLength * 2, Encoding.Unicode);

            return new()
            {
                Message = "<Auto message>",
                Author = messageAuthor,
                Type = AuthorType.Player1
            };
        }

        private MHRChatMessage DerefUnknownTypeMessage(long messagePtr) => new() { Type = AuthorType.None };

        #endregion
    }
#pragma warning restore IDE0051 // Remove unused private members
}
