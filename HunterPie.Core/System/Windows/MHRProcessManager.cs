using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Logger;
using System.Diagnostics;
using System.IO;

namespace HunterPie.Core.System.Windows
{
    internal class MHRProcessManager : WindowsProcessManager
    {

        public override string Name => "MonsterHunterRise";
        public override GameProcess Game => GameProcess.MonsterHunterRise;

        protected override bool ShouldOpenProcess(Process process)
        {
            if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER RISE"))
                return false;

            string riseVersion;
            try
            {
                riseVersion = process.MainModule.FileVersionInfo.FileVersion;
            } catch
            {
                Log.Error("Failed to get Monster Hunter Rise version, missing permissions. Try running as administrator.");
                ShouldPollProcess = false;
                return false;
            }

            Log.Info($"Detected Monster Hunter Rise version: {riseVersion}");

            AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterRise.{riseVersion}.map"));

            if (!AddressMap.IsLoaded)
            {
                Log.Error("Failed to load address for Monster Hunter Rise v{0}", riseVersion);
                ShouldPollProcess = false;
                return false;
            }

            return true;
        }
    }
}
