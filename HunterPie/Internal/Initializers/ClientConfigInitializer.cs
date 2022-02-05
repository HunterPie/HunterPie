using HunterPie.Core.Client;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Initializers
{
    internal class ClientConfigInitializer : IInitializer
    {
        public void Init()
        {
            ClientConfig.Initialize();
        }
    }
}
