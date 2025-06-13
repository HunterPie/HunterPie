using HunterPie.Core.Domain.Enums;
using HunterPie.Core.Game;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Overlay;

internal interface IWidgetInitializer
{
    public GameProcessType SupportedGames { get; }

    public Task LoadAsync(IContext context);
    public void Unload();
}