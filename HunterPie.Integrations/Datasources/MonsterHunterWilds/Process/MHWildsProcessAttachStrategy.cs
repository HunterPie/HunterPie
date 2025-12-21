using HunterPie.Core.Address.Map;
using HunterPie.Core.Client;
using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Domain.Process;
using HunterPie.Core.Domain.Process.Exceptions;
using HunterPie.Core.Domain.Process.Service;
using HunterPie.Core.Extensions;
using HunterPie.Core.Game.Events;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Integrations.Datasources.MonsterHunterWilds.Process;

internal class MHWildsProcessAttachStrategy : IProcessAttachStrategy
{
    public string Name => SupportedGameNames.MONSTER_HUNTER_WILDS;

    public GameProcessType Game => GameProcessType.MonsterHunterWilds;

    private ProcessStatus _status;

    public ProcessStatus Status
    {
        get => _status;
        private set
        {
            if (_status == value)
                return;

            ProcessStatus oldStatus = _status;
            _status = value;

            this.Dispatch(
                StatusChange,
                new SimpleValueChangeEventArgs<ProcessStatus>(oldStatus, value)
            );
        }
    }
    public event EventHandler<SimpleValueChangeEventArgs<ProcessStatus>>? StatusChange;

    public bool CanAttach(SystemProcess process)
    {
        if (!process.MainWindowTitle.ToUpperInvariant().StartsWith("MONSTER HUNTER WILDS"))
            return false;

        string? version = process.MainModule
            ?.FileVersionInfo.FileVersion;

        if (version is not { })
            throw new UnauthorizedAccessException("Failed to get Monster Hunter Wilds version, missing permissions");

        string mapPath = Path.Combine(ClientInfo.AddressPath, $"MonsterHunterWilds.{version}.map");

        if (!Path.Exists(mapPath))
            throw new UnsupportedGamePatchException(Name, version);

        bool hasLoaded = AddressMap.Parse(
            filePath: mapPath
        );

        if (!hasLoaded)
            throw new Exception($"Failed to load address for Monster Hunter Wilds v{version}");

        return true;
    }

    public void SetStatus(ProcessStatus status) => Status = status;
}