using HunterPie.Core.API;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Initializers
{
    internal class ApiPingInitializer : IInitializer
    {
        public void Init() => PoogieApi.GetSession();
    }
}
