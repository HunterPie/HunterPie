using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;

namespace HunterPie.UI.Overlay
{
    public interface IWidgetWindow
    {
        public WidgetType Type { get; }
        public IWidgetSettings Settings { get; }
    }
}
