using HunterPie.Core.Address.Map;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Definitions;
using System.Collections.Generic;
using System.Linq;

namespace HunterPie.Core.Game.Data
{
    public class SongSkill
    {
        private CMusicSkillData[] _data;
        private readonly IProcessManager _process;
        private static SongSkill _instance;

        public IReadOnlyCollection<CMusicSkillData> Data => _data;

        #region Public API

        /// <summary>
        /// Gets song data structure based on song id
        /// </summary>
        /// <param name="songId">SongId</param>
        /// <returns></returns>
        public static CMusicSkillData? GetMusicSkillData(int songId)
        {
            return _instance._data.ElementAtOrDefault(songId + 1);
        }

        #endregion


        internal SongSkill(IProcessManager process)
        {
            _process = process;
            _instance = this;

            Load();
        }

        private void Load()
        {
            long address = _process.Memory.Read(
                AddressMap.GetAbsolute("MUSIC_SKILL_EFC_DATA_OFFSET"),
                AddressMap.Get<int[]>("cMusicSkillEfcDataOffsets")
            );

            int nElements = _process.Memory.Read<int>(address - 0x4);

            long[] buffer = _process.Memory.Read<long>(address, (uint)nElements);
            _data = new CMusicSkillData[nElements];

            for (int i = 0; i < buffer.Length; i++)
                _data[i] = _process.Memory.Read<CMusicSkillData>(buffer[i]);
        }
    }
}
