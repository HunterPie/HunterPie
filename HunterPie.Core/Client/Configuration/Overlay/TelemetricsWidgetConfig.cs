using HunterPie.Core.Architecture;
using HunterPie.Core.Settings;
using HunterPie.Core.Settings.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.Core.Client.Configuration.Overlay
{
    public class TelemetricsWidgetConfig : IWidgetSettings
    {
        [SettingField("A", "B")]
        public Observable<bool> Initialize { get; set; } = true;

        [SettingField("A", "B")]
        public Observable<bool> Enabled { get; set; } = true;

        [SettingField("A", "B")]
        public Position Position { get; set; } = new(100, 100);

        [SettingField("A", "B")]
        public Observable<double> Opacity { get; set; } = 1;

        [SettingField("A", "B")]
        public Observable<double> Scale { get; set; } = 1;

        [SettingField("A", "B")]
        public Observable<bool> StreamerMode { get; set; } = false;
    }
}
