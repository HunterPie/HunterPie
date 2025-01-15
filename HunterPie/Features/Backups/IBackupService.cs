using HunterPie.Integrations.Poogie.Common.Models;
using System.Threading.Tasks;

namespace HunterPie.Features.Backups;
internal interface IBackupService
{
    public GameType Type { get; }

    /// <summary>
    /// Executes a backup and returns the path to the backup file
    /// </summary>
    /// <returns>Path to the backup file</returns>
    public Task<string> ExecuteAsync();
}