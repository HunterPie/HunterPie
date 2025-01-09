using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Service;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterRise.Process;

internal class MHRProcessAttachStrategy : IProcessAttachStrategy
{
    public string Name => SupportedGameNames.MONSTER_HUNTER_RISE;

    public GameProcessType Game => GameProcessType.MonsterHunterRise;

    public bool CanAttach(SystemProcess process)
    {
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER RISE"))
            return false;

        string? version = process.MainModule?.FileVersionInfo.FileVersion;

        if (version is not { })
            throw new UnauthorizedAccessException("Failed to get Monster Hunter Rise version, missing permissions");

        bool hasLoaded = AddressMap.Parse(
            filePath: Path.Combine(ClientInfo.AddressPath, $"MonsterHunterRise.{version}.map")
        );

        if (!hasLoaded)
            throw new Exception($"Failed to load address for Monster Hunter Rise v{version}");

        return true;
    }
}