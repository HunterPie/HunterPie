using HunterPie.Core.Client.Localization;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Internal.Initializers
{
    internal class ClientLocalizationInitializer : IInitializer
    {
        public void Init()
        {
            _ = Localization.Instance;
        }
    }
}
