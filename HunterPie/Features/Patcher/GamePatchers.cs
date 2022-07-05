using HunterPie.Core.Game;
using HunterPie.Domain.Interfaces;

namespace HunterPie.Features.Patcher
{
    internal static class GamePatchers
    {
        private static IContextInitializer[] patchers = new IContextInitializer[]
        {
            new RiseIntegrityPatcher(),
        };

        public static void Run(Context context)
        {
            foreach (var patcher in patchers)
                patcher.Initialize(context);
        }
    }
}
