namespace HunterPie.Core.Settings
{
    public interface IWidgetSettings
    {
        bool Initialize { get; set; }
        bool Enabled { get; set; }
        double[] Position { get; set; }
        double Opacity { get; set; }
        double Scale { get; set; }
        bool StreamerMode { get; set; }
    }
}
