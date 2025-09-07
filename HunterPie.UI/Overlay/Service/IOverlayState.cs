namespace HunterPie.UI.Overlay.Service;

public interface IOverlayState
{
    public bool IsDesignModeEnabled { get; }
    public bool IsGameHudVisible { get; }
    public bool IsGameFocused { get; }
}