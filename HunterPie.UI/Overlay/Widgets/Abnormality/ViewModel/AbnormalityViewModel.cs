using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Abnormality.ViewModel;

public class AbnormalityViewModel : Bindable
{
    public string Icon
    {
        get;
        set => SetValue(ref field, value);
    }
    public string Id
    {
        get;
        set => SetValue(ref field, value);
    }
    public string Name
    {
        get;
        set => SetValue(ref field, value);
    }
    public double Timer
    {
        get;
        set => SetValue(ref field, value);
    }
    public double MaxTimer
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsBuff
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsBuildup
    {
        get;
        set => SetValue(ref field, value);
    }
    public bool IsInfinite { get; set => SetValue(ref field, value); }
}