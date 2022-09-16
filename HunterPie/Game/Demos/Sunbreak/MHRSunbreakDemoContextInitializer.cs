using System.Threading.Tasks;
using HunterPie.Domain.Interfaces;
using HunterPie.Core.Game;
using HunterPie.Core.Game.Demos.Sunbreak;

namespace HunterPie.Game.Demos.Sunbreak;

internal class MHRSunbreakDemoContextInitializer : IContextInitializer
{

    /// <inheritdoc />
    public async Task InitializeAsync(Context context)
    {
        if (context is not MHRSunbreakDemoContext) return;

        // Nothing to initialize.
    }

}
