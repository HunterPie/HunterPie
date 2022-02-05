using HunterPie.Core.Logger;
using HunterPie.Domain.Interfaces;
using HunterPie.UI.Logger;

namespace HunterPie.Internal.Initializers
{
    internal class HunterPieLoggerInitializer : IInitializer
    {
        public void Init()
        {
            ILogger logger = new HunterPieLogger();
            Log.Add(logger);

            Log.Info("Initialized HunterPie logger");
        }
    }
}
