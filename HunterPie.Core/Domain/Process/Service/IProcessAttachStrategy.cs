using HunterPie.Core.Domain.Enums;
using SystemProcess = System.Diagnostics.Process;

namespace HunterPie.Core.Domain.Process.Service;

public interface IProcessAttachStrategy
{
    public string Name { get; }
    public GameProcessType Game { get; }
    public bool CanAttach(SystemProcess process);
}