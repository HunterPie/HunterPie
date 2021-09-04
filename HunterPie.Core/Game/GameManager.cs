using HunterPie.Core.Domain;
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

        private readonly IProcessManager processManager;

        #endregion

        public Player Player { get; private set; }


        internal GameManager(IProcessManager process)
        {
            processManager = process;
        }

        private void CreateEntities()
        {
            Player = new Player(processManager);
        }

        internal void SetupScanners()
        {
            CreateEntities();

            ScanManager.Add(
                Player    
            );

            ScanManager.Start();
        }
    }
}
