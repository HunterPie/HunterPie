using HunterPie.Core.Game;

namespace HunterPie.UI.Architecture.Overlay;

internal interface IWidgetInitializer
{
    public void Load(Context context);
    public void Unload();
}
