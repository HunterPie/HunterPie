using HunterPie.Core.Architecture;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatElementViewModel : Bindable
{
    public string Text { get; set => SetValue(ref field, value); }
    public string Author { get; set => SetValue(ref field, value); }
    public int Index { get; set => SetValue(ref field, value); }
}