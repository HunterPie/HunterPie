using System.Threading.Tasks;

namespace HunterPie.Features.Account.Config;

internal interface IRemoteAccountConfigUseCase
{
    Task Upload();
    Task Download();
}