using System.Threading.Tasks;

namespace HunterPie.Update.Usecase;

public interface IUpdateUseCase
{
    Task<bool> InvokeAsync();
}