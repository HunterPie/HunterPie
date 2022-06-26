using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Logger;
using System.Diagnostics;
using System.IO;

namespace HunterPie.Core.System.Windows
{
    internal class MHRSunbreakDemoProcessManager : WindowsProcessManager
    {

        public override string Name => "MHRiseSunbreakDemo";
        public override GameProcess Game => GameProcess.MonsterHunterRiseSunbreakDemo;

        protected override bool ShouldOpenProcess(Process process)
        {
            if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER RISE: SUNBREAK DEMO"))
                return false;

            string riseVersion;
            try
            {
                riseVersion = process.MainModule.FileVersionInfo.FileVersion;
            }
            catch
            {
                Log.Error("Failed to get Monster Hunter Rise: Sunbreak [DEMO] version, missing permissions. Try running as administrator.");
                ShouldPollProcess = false;
                return false;
            }

            Log.Info($"Detected Monster Hunter Rise: Sunbreak [DEMO] version: {riseVersion}");

            AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterSunbreakDemo.{riseVersion}.map"));

            if (!AddressMap.IsLoaded)
            {
                Log.Error("Failed to load address for Monster Hunter Rise: Sunbreak [DEMO] v{0}", riseVersion);
                ShouldPollProcess = false;
                return false;
            }

            return true;
        }
    }
}
