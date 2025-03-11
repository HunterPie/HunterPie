using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Domain.Memory;

public interface IMemoryReaderAsync
{
    public Task<string> ReadAsync(
        nint address,
        int length,
        Encoding? encoding = null);

    public Task<T> ReadAsync<T>(nint address) where T : struct;

    public Task<T[]> ReadAsync<T>(nint address, int count) where T : struct;

    public Task<nint> ReadPtrAsync(nint address, int[] offsets);

    public Task<nint> ReadAsync(nint address, int[] offsets);

    public Task<T> DerefAsync<T>(nint address, int[] offsets) where T : struct;

    public Task<T> DerefPtrAsync<T>(nint address, int[] offsets) where T : struct;
}