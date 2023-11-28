using HunterPie.Core.Architecture;

namespace HunterPie.UI.Controls.Settings.Custom.Abnormality;

public class AbnormalityViewModel : Bindable
{

    public string Name { get; set; }
    public string Icon { get; set; }
    public string Id { get; set; }
    public string Category { get; set; }

    private bool _isEnabled;
    public bool IsEnabled { get => _isEnabled; set => SetValue(ref _isEnabled, value); }

    private bool _isMatch;
    public bool IsMatch { get => _isMatch; set => SetValue(ref _isMatch, value); }
}
