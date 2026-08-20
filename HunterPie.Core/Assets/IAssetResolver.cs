using System.Threading.Tasks;

namespace HunterPie.Core.Assets;

public interface IAssetResolver
{
    public Task<string> Resolve(string path);
}
