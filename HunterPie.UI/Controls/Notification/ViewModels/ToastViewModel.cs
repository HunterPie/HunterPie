using HunterPie.Core.Notification.Model;
using HunterPie.UI.Architecture;
using System;

namespace HunterPie.UI.Controls.Notification.ViewModels;

#nullable enable
public class ToastViewModel : ViewModel
{
    public NotificationType Type { get; set => SetValue(ref field, value); }
    public string Title { get; set => SetValue(ref field, value); } = string.Empty;
    public string Description { get; set => SetValue(ref field, value); } = string.Empty;
    public string? PrimaryLabel { get; set => SetValue(ref field, value); }
    public EventHandler<EventArgs>? PrimaryHandler { get; set => SetValue(ref field, value); }
    public string? SecondaryLabel { get; set => SetValue(ref field, value); }
    public EventHandler<EventArgs>? SecondaryHandler { get; set => SetValue(ref field, value); }
    public int ButtonCount { get; set => SetValue(ref field, value); }
    public bool IsVisible { get; set => SetValue(ref field, value); } = true;
}