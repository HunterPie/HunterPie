using HunterPie.Core.Architecture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality
{
    public class AbnormalityViewModel : Bindable
    {
        private bool _isEnabled;

        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsEnabled { get => _isEnabled; set { SetValue(ref _isEnabled, value); } }
    }
}
