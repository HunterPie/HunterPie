using HunterPie.Core.Architecture;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality
{
    public class AbnormalityViewModel : Bindable
    {
        private bool _isEnabled;

        public string Name { get; set; }
        public string Icon { get; set; }
        public string Id { get; set; }
        public bool IsEnabled { get => _isEnabled; set { SetValue(ref _isEnabled, value); } }
    }
}
