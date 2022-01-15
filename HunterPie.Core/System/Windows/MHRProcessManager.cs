using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using System.Diagnostics;
using System.IO;

namespace HunterPie.Core.System.Windows
{
    internal class MHRProcessManager : WindowsProcessManager
    {

        public override string Name => "MonsterHunterRise";

        protected override bool ShouldOpenProcess(Process process)
        {
            if (!process.MainWindowTitle.ToUpper().StartsWith("MONSTERHUNTERRISE"))
                return false;

            // TODO: Rise versioning

            AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, "MonsterHunterRise.0.map"));

            if (!AddressMap.IsLoaded)
                return false;

            return true;
        }
    }
}
