using HunterPie.Core.Settings;

namespace HunterPie.UI.Overlay;

public interface IWidget<out T> where T : IWidgetSettings
{
    public T Settings { get; }
    public string Title { get; }
}