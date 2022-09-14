using System.Threading.Tasks;
using HunterPie.Core.Game;
using HunterPie.Domain.Interfaces;
using HunterPie.Game.Demos.Sunbreak;
using HunterPie.Game.Rise;
using HunterPie.Game.World;

namespace HunterPie.Features
{
    internal static class ContextInitializers
    {
        private static IContextInitializer[] initializers = new IContextInitializer[]
        {
            new MHWContextInitializer(),
            new MHRContextInitializer(),
            new MHRSunbreakDemoContextInitializer(),
        };

        public static async Task InitializeAsync(Context context)
        {
            foreach (var initializer in initializers)
                await initializer.InitializeAsync(context);
        }
    }
}
