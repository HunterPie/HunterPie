namespace HunterPie.UI.Controls.Settings.Abnormality.ViewModels;

public class AbnormalityElementViewModel : Architecture.ViewModel
{
    public string Id { get; set => SetValue(ref field, value); }
    public string Icon { get; set => SetValue(ref field, value); }
    public string Name { get; set => SetValue(ref field, value); }
    public string Category { get; set => SetValue(ref field, value); }
    public bool IsEnabled { get; set => SetValue(ref field, value); }
    public bool IsMatch { get; set => SetValue(ref field, value); }
}