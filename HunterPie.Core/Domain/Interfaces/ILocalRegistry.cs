using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Interfaces;
public interface ILocalRegistry
{

    public Task SetAsync<T>(string name, T value) where T : class;
    public Task<bool> ExistsAsync(string name);
    public Task<object?> GetAsync(string name);
    public Task<T?> GetAsync<T>(string name) where T : class;
    public Task DeleteAsync(string name);
}