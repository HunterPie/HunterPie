using HunterPie.Core.Settings;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.ViewModels;
using System.Collections.ObjectModel;

namespace HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

public class ChatViewModel : WidgetViewModel
{
    private bool _isChatOpen;
    public bool IsChatOpen { get => _isChatOpen; set => SetValue(ref _isChatOpen, value); }

    public ObservableCollection<ChatCategoryViewModel> Categories { get; } = new();

    public ChatViewModel(IWidgetSettings settings) : base(settings, "Chat Widget", WidgetType.ClickThrough)
    {

    }
}