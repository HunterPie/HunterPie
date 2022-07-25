using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    [SettingsGroup("ABNORMALITY_TRAY_STRING", "ICON_SELFIMPROVEMENT+")]
    public class AbnormalityTrayConfig : ISettings
    {
        [SettingField("ABNORMALITY_TRAYS_STRING", requiresRestart: true)]
        public AbnormalityTrays Trays { get; set; } = new();
    }
}
