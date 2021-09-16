using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Data;

namespace HunterPie.Core.Game
{
    public class Context
    {
        public readonly GameManager Game;
        public readonly Strings Strings;
        public readonly SongSkill SongsSkill;
        public readonly IProcessManager Process;

        internal Context(
                IProcessManager process,
                Strings strings,
                SongSkill songSkill,
                GameManager game
        )
        {
            Process = process;
            Game = game;
            Strings = strings;
            SongsSkill = songSkill;

            Game.SetupScanners();
        }
    }
}
