using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Definitions;
using HunterPie.Core.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Game.Data
{
    public class Strings
    {
        public struct GMD
        {
            public long KeysBaseAddress;
            public int KeysChunkSize;
            public int KeysCount;
            public Dictionary<string, int> Keys;

            public long ValuesBaseAddress;
            public int[] ValuesOffsets;
            public int ValuesChunkSize;
        }
        private IProcessManager _process;
        private static Strings _instance;

        private GMD _itemsGmd = new();
        private GMD _abnormalityGmd = new();
        private GMD _endemicGmd = new();

        public ref readonly GMD Items => ref _itemsGmd;
        public ref readonly GMD Abnormalities => ref _abnormalityGmd;
        public ref readonly GMD Endemic => ref _endemicGmd;

        private static readonly Dictionary<string, string> _specialEndemics = new()
        {
            { "EM032_01", "EM_TEST_00002" },
            { "EM043_05", "EM_TEST_00001" },
            { "EM120_00", "EM107_01" },
            { "EM121_00", "EM121_01" }
        };

        internal Strings(IProcessManager process)
        {
            _process = process;
            _instance = this;
        }

        #region Public API

        /// <summary>
        /// Gets item name in the language the game is currently using
        /// </summary>
        /// <param name="itemId">Item Id</param>
        /// <returns>The item name</returns>
        public static string GetItemNameById(int itemId)
        {
            if (itemId * 2 >= _instance._itemsGmd.ValuesOffsets.Length)
                return null;

            return GetRawValueString(_instance._itemsGmd, itemId * 2);
        }

        /// <summary>
        /// Gets item description in the language the game is currently using
        /// </summary>
        /// <param name="itemId">Item id</param>
        /// <returns>The item description</returns>
        public static string GetItemDescriptionById(int itemId)
        {
            return GetRawValueString(_instance._itemsGmd, itemId * 2 + 1).RemoveChars();
        }

        /// <summary>
        /// Gets monster name by Em
        /// </summary>
        /// <param name="monsterEm">Monster em</param>
        /// <returns>Monster name</returns>
        public static string GetMonsterNameByEm(string monsterEm)
        {
            if (monsterEm is null)
                return null;

            // Capcom, if you're reading this
            // WHY????? JUST WHY??????
            monsterEm = monsterEm.ToUpperInvariant();

            if (!_instance._endemicGmd.Keys.ContainsKey(monsterEm))
            {
                if (_specialEndemics.ContainsKey(monsterEm))
                    monsterEm = _specialEndemics[monsterEm];
                else
                {
                    monsterEm = monsterEm.Split('_').First();

                    if (!_instance._endemicGmd.Keys.ContainsKey(monsterEm))
                        return null;
                }
            }

            int idx = _instance._endemicGmd.Keys[monsterEm];

            return GetRawValueString(_instance._endemicGmd, idx).FilterStyles();
        }

        /// <summary>
        /// Get monster description by their Em
        /// </summary>
        /// <param name="monsterEm">Monster em</param>
        /// <returns>Monster description</returns>
        public static string GetMonsterDescriptionByEm(string monsterEm)
        {
            if (monsterEm is null)
                return null;

            monsterEm = monsterEm.ToUpperInvariant();

            if (!_instance._endemicGmd.Keys.ContainsKey(monsterEm))
            {
                if (_specialEndemics.ContainsKey(monsterEm))
                    monsterEm = _specialEndemics[monsterEm];
                else
                {
                    monsterEm = monsterEm.Split('_').First();

                    if (!_instance._endemicGmd.Keys.ContainsKey(monsterEm))
                        return null;
                }

            }

            monsterEm += "_DESC";
            int idx = _instance._endemicGmd.Keys[monsterEm];
            return GetRawValueString(_instance._endemicGmd, idx).RemoveChars();
        }

        /// <summary>
        /// Returns song name based on its Id
        /// </summary>
        /// <param name="songId">Song Id</param>
        /// <returns>Song name</returns>
        public static string GetMusicSkillNameById(int songId)
        {

            CMusicSkillData? data = SongSkill.GetMusicSkillData(songId);

            if (data is null)
                return null;

            songId = ((CMusicSkillData)data).StringId;

            return GetRawValueString(_instance._abnormalityGmd, songId);
        }

        /// <summary>
        /// Gets raw string from a GMD file
        /// </summary>
        /// <param name="gmd">the GMD that will be searched for</param>
        /// <param name="idx">Index in the value</param>
        /// <returns>Raw string read from memory</returns>
        public static string GetRawValueString(GMD gmd, int idx)
        {
            if (idx >= gmd.ValuesOffsets.Length || idx < 0)
                return "Unknown";

            if (idx >= gmd.ValuesOffsets.Length || idx < 0)
            {
                return "";
            }
            long length;
            if ((idx + 1) >= gmd.ValuesOffsets.Length)
            {
                length = gmd.ValuesChunkSize - gmd.ValuesOffsets[idx];
            }
            else
            {
                length = gmd.ValuesOffsets[idx + 1] - gmd.ValuesOffsets[idx];
            }

            long stringAddress = gmd.ValuesBaseAddress + gmd.ValuesOffsets[idx];

            return _instance._process.Memory.Read(stringAddress, (uint)length);

        }
        #endregion

        internal async Task<bool> InitializeGMDs()
        {
            bool ready = false;

            List<int> initialized = new();
            List<Func<bool>> initializers = new()
            {
                InitializeItemsGmd,
                InitializeEndemicsGmd,
                InitializeAbnormalitiesGmd
            };

            while (!ready)
            {
                int idx = 0;
                foreach (Func<bool> initializer in initializers)
                {
                    if (initializer())
                        initialized.Add(idx);
                }

                foreach (int i in initialized)
                    initializers.RemoveAt(i);

                initialized.Clear();

                if (initializers.Count == 0)
                    return true;

                await Task.Delay(1000);
            }

            return true;
        }

        private bool InitializeItemsGmd()
        {
            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("GMD_ITEMS_OFFSET"),
                AddressMap.Get<int[]>("GmdItemsOffsets")
            );
            return LoadGmd(ref _itemsGmd, address);
        }

        private bool InitializeEndemicsGmd()
        {
            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("GMD_MONSTERS_OFFSET"),
                AddressMap.Get<int[]>("GmdOffsets")
            );

            bool initialized = LoadGmd(ref _endemicGmd, address);

            if (initialized)
                LoadGmdKeys(ref _endemicGmd);

            return initialized;
        }

        private bool InitializeAbnormalitiesGmd()
        {
            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("GMD_BUFFS_OFFSET"),
                AddressMap.Get<int[]>("GmdOffsets")
            );
            return LoadGmd(ref _abnormalityGmd, address);
        }

        private bool LoadGmd(ref GMD gmd, long address)
        {
            if (address == 0x00000000)
                return false;

            long addressBase = _process.Memory.Read<long>(address);

            // GMD Metadata 
            // TODO: Turn this into a structure
            string gmdName = _process.Memory.Read(address - 0xEC, 32);
            int nElements = _process.Memory.Read<int>(address - 0x40);
            gmd.ValuesChunkSize = _process.Memory.Read<int>(address - 0x10);
            gmd.KeysChunkSize = _process.Memory.Read<int>(address - 0x30);
            gmd.KeysBaseAddress = _process.Memory.Read<long>(address - 0x28);
            gmd.KeysCount = _process.Memory.Read<int>(address - 0x20);

            // An array of pointers to the strings, this way we can index each element without
            // calculating the string length first
            long[] valueStringsPtrs = _process.Memory.Read<long>(addressBase, (uint)nElements);

            long @base = AddressMap.Get<long>("BASE");
            
            gmd.ValuesOffsets = valueStringsPtrs
                // Filter all pointers that are pointing to a static address
                .Where(ptr => (ptr & @base) != @base)
                // Calculate the string offset based on the distance it is to the first string
                .Select(str => (int)(str - valueStringsPtrs[0]))
                .ToArray();

            gmd.ValuesBaseAddress = _process.Memory.Read<long>(addressBase);

            Log.Debug($"Indexed {gmdName} (Strings: {nElements} | Chunk size: {gmd.ValuesChunkSize} bytes)");

            return true;
        }

        private void LoadGmdKeys(ref GMD gmd)
        {
            gmd.Keys?.Clear();

            gmd.Keys = new Dictionary<string, int>(gmd.KeysCount);
            byte[] chars = _process.Memory.Read<byte>(gmd.KeysBaseAddress, (uint)gmd.KeysChunkSize);

            StringBuilder sb = new();

            int idx = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] != 0)
                {
                    sb.Append((char)chars[i]);
                } else
                {
                    gmd.Keys[sb.ToString()] = idx;
                    idx++;
                    sb.Clear();
                    continue;
                }
            }
        }
    }
}
