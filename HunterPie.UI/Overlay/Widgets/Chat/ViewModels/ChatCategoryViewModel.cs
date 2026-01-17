using HunterPie.Core.Architecture;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatCategoryViewModel : Bindable
{
    public string Name { get; set => SetValue(ref field, value); }
    public string Description { get; set => SetValue(ref field, value); }
    public ImageSource Icon { get; set => SetValue(ref field, value); }
    public ObservableCollection<ChatElementViewModel> Elements { get; } = new();
}