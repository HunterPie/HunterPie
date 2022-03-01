using HunterPie.Core.Domain.Process;
using HunterPie.Core.Game.Rise.Data;

namespace HunterPie.Core.Game.Rise
{
    public class MHRContext : Context
    {

        private static MHRStrings _strings;
        public static MHRStrings Strings => _strings;

        internal MHRContext(IProcessManager process)
        {
            _strings = new MHRStrings(process);
            Game = new MHRGame(process);
            Process = process;
        }

        internal override void Scan()
        {
            if (Game is MHRGame game)
                game.StartScanTask();
        }

        internal override void Stop()
        {
            if (Game is MHRGame game)
                game.StopScanning();
        }
    }
}
