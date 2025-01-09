using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process.Service;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Process;

internal class MHWProcessAttachStrategy : IProcessAttachStrategy
{
    public string Name => SupportedGameNames.MONSTER_HUNTER_WORLD;

    public GameProcessType Game => GameProcessType.MonsterHunterWorld;

    public bool CanAttach(SystemProcess process)
    {
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER: WORLD"))
            return false;

        string? rawVersion = process.MainWindowTitle.Split('(')
            .ElementAtOrDefault(1)
            ?.TrimEnd(')');
        bool isValidVersion = int.TryParse(rawVersion, out int version);

        if (!isValidVersion)
            throw new UnauthorizedAccessException("Failed to get Monster Hunter World version");

        bool hasLoaded = IsIce(version)
            ? AddressMap.ParseLatest(ClientInfo.AddressPath)
            : AddressMap.Parse(Path.Combine(ClientInfo.AddressPath, $"MonsterHunterWorld.{rawVersion}.map"));

        if (!hasLoaded)
            throw new Exception($"Failed to load address for Monster Hunter World v{version}");

        return true;
    }

    private bool IsIce(int version) => version is >= 300_000 and <= 400_000;
}