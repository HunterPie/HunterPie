using HunterPie.Core.Game;
using HunterPie.Core.Game.Entity.Game.Chat;
using HunterPie.Core.Game.Enums;
using HunterPie.UI.Assets.Application;
using HunterPie.UI.Overlay.Enums;
using HunterPie.UI.Overlay.Widgets.Chat.ViewModels;

namespace HunterPie.UI.Overlay.Widgets.Chat;

public class ChatWidgetContextHandler : IContextHandler
{
    private readonly ChatViewModel _viewModel;
    private readonly IContext _context;
    public ChatCategoryViewModel General { get; } = new()
    {
        Name = "General",
        Description = "General chat",
        Icon = Resources.Icon("ICON_STAR")
    };

    public ChatWidgetContextHandler(
        IContext context,
        ChatViewModel viewModel)
    {
        _context = context;
        _viewModel = viewModel;

        UpdateData();
        HookEvents();
    }
    private void UpdateData()
    {
        _viewModel.IsChatOpen = _context.Game.Chat!.IsChatOpen;
        _viewModel.Type = _viewModel.IsChatOpen
            ? WidgetType.Window
            : WidgetType.ClickThrough;

        _viewModel.Categories.Add(General);

        foreach (IChatMessage message in _context.Game.Chat.Messages)
            _viewModel.UIThread.Invoke(() =>
            {
                if (message.Type != AuthorType.Player)
                    return;

                General.Elements.Add(new ChatElementViewModel()
                {
                    Author = message.Author,
                    Text = message.Message,
                    Index = message.PlayerSlot
                });
            });
    }

    public void HookEvents()
    {
        _context.Game.Chat!.OnNewChatMessage += OnNewChatMessage;
        _context.Game.Chat.OnChatOpen += OnChatOpen;
    }

    private void OnChatOpen(object sender, IChat e)
    {
        _viewModel.IsChatOpen = e.IsChatOpen;
        _viewModel.Type = e.IsChatOpen
            ? WidgetType.Window
            : WidgetType.ClickThrough;
    }

    public void UnhookEvents()
    {
        _context.Game.Chat!.OnNewChatMessage -= OnNewChatMessage;
        _context.Game.Chat.OnChatOpen -= OnChatOpen;
    }

    private void OnNewChatMessage(object sender, IChatMessage e)
    {
        _viewModel.UIThread.Invoke(() =>
        {
            if (e.Type != AuthorType.Player)
                return;

            General.Elements.Add(new ChatElementViewModel
            {
                Author = e.Author,
                Text = e.Message,
                Index = e.PlayerSlot
            });
        });
    }
}