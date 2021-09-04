using HunterPie.Core.Domain.Process;

namespace HunterPie.Core.Game
{
    public class Context
    {
        public readonly GameManager Game;
        public readonly IProcessManager Process;

        internal Context(
                IProcessManager process,
                GameManager game
        )
        {
            Process = process;
            Game = game;

            Game.SetupScanners();
        }
    }
}
