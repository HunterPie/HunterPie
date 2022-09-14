using System.Threading.Tasks;
using HunterPie.Core.Game;

namespace HunterPie.Domain.Interfaces
{
    internal interface IContextInitializer
    {
        public Task InitializeAsync(Context context);
    }
}
