using System.Threading.Tasks;

namespace HunterPie.Update;

public interface IUpdateUseCase
{
    Task<bool> Invoke();
}