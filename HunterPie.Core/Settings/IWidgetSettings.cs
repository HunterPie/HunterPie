using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Settings
{
    public interface IWidgetSettings
    {
        Observable<bool> Initialize { get; set; }
        Observable<bool> Enabled { get; set; }
        Position Position { get; set; }
        Observable<double> Opacity { get; set; }
        Observable<double> Scale { get; set; }
        Observable<bool> StreamerMode { get; set; }
    }
}
