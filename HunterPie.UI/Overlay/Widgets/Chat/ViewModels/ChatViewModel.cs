using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatViewModel(IWidgetSettings settings) : WidgetViewModel(settings, "Chat Widget", WidgetType.ClickThrough)
{
    public bool IsChatOpen { get; set => SetValue(ref field, value); }

    public ObservableCollection<ChatCategoryViewModel> Categories { get; } = new();
}