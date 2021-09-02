using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Client;

namespace HunterPie.Core.Game
{
    /// <summary>
    /// Manager responsible to sync in-game entities
    /// </summary>
    public class GameManager
    {
        #region Private fields

        private IProcessManager processManager;

        #endregion

        public Player Player { get; private set; }


        internal GameManager(IProcessManager process)
        {

        }
    }
}
