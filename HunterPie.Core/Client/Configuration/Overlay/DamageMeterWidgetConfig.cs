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
    public class DamageMeterWidgetConfig : IWidgetSettings
    {
        [SettingField("MOCK", "MOCK")]
        public Observable<bool> Initialize { get; set; } = true;
        
        [SettingField("MOCK", "MOCK")]
        public Observable<bool> Enabled { get; set; } = true;
        
        [SettingField("MOCK", "MOCK")]
        public Position Position { get; set; } = new(0, 0);
        
        [SettingField("MOCK", "MOCK")]
        public Observable<double> Opacity { get; set; } = 1;
        
        [SettingField("MOCK", "MOCK")]
        public Observable<double> Scale { get; set; } = 1;
        
        [SettingField("MOCK", "MOCK")]
        public Observable<bool> StreamerMode { get; set; } = false;
    }
}
