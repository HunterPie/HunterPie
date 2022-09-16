using HunterPie.Core.Game;
using HunterPie.Core.Game.World;
using HunterPie.Domain.Interfaces;
using System.Threading.Tasks;

namespace HunterPie.Game.World;

internal class MHWContextInitializer : IContextInitializer
{

    /// <inheritdoc />
    public async Task InitializeAsync(Context context)
    {
        if (context is not MHWContext)
            return;

        // Nothing to initialize, yet.
    }
}
