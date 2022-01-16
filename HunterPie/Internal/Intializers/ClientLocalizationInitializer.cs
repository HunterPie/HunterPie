using HunterPie.Core.Client.Localization;
using HunterPie.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Internal.Intializers
{
    internal class ClientLocalizationInitializer : IInitializer
    {
        public void Init()
        {
            _ = Localization.Instance;
        }
    }
}
